#include <iostream>  //入出力関連ヘッダ
#include <opencv2/opencv.hpp>  //OpenCV関連ヘッダ
#include <arpa/inet.h>
#include <netinet/in.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <unistd.h>
#include <string>
#include <fstream>  // ファイルの入出力
#include <limits>  // 数値の最大値を取得する

#define FILE_NAME "index.txt"  //ファイル（動作確認用）

// カメラ設定
const int CAMERA_INDEX = 2;
const cv::Size FRAME_SIZE(1280, 720); // 幅と高さの設定（調整）

int main (int argc, char *argv[])
{
    // データ送信
    int sock;
    struct sockaddr_in addr;

    sock = socket(AF_INET, SOCK_DGRAM, 0);

    addr.sin_family = AF_INET;
    addr.sin_port = htons(12345);
    addr.sin_addr.s_addr = inet_addr("127.0.0.1");


    //ビデオキャプチャ初期化
    cv::VideoCapture capture(CAMERA_INDEX);  //カメラをオープン
    //ビデオファイルがオープンできたかどうかをチェック
    if (capture.isOpened()==0) {
        std::cerr << "Error: Could not open camera." << std::endl;
        return -1;
    }

    // フレームサイズの設定
    capture.set(cv::CAP_PROP_FRAME_WIDTH, FRAME_SIZE.width); // 幅
    capture.set(cv::CAP_PROP_FRAME_HEIGHT, FRAME_SIZE.height); // 高さ

    // カメラのホワイトバランスと露出の設定
    capture.set(cv::CAP_PROP_AUTO_WB, false);  // 自動ホワイトバランスを無効化
    capture.set(cv::CAP_PROP_AUTO_EXPOSURE, 0.25);  // 露出を下げる
    capture.set(cv::CAP_PROP_BRIGHTNESS, 0.5);  // 明るさを調整

    //画像格納用インスタンス準備
    int imageW = 1500, imageH = 800;
    cv::Mat originalImage;  //カメラオリジナル映像
    cv::Mat camImage(cv::Size(imageW,imageH), CV_8UC3);  //処理用リサイズ映像
    cv::Mat hsvImage;  //処理用HSV映像

    //画像表示用ウィンドウの生成
    cv::namedWindow("Cam");

    //検出領域＆格子設定
    //※必要に応じて数値を調整
    int areaW = 1470, areaH = 600;  //検出領域サイズ
    int M=9, N=4;  //検出領域分割数
    int gap = 60;  // 格子間の隙間
    cv::Point areaC(677, 405);  //検出領域中心
    double latticeW = (areaW - (M - 1) * gap) / M, latticeH = (areaH - (N - 1) * gap) / N;  //格子サイズ
    std::vector<std::vector<cv::Rect>> latticeRect;  //各格子

    for (int i=0; i<M; i++) {
        latticeRect.push_back(std::vector<cv::Rect>());  //i行追加
        for (int j=0; j<N; j++) {
            cv::Rect temp;
            // 隙間の分を考慮してx座標を設定
            int gap_offset = (i / 3) * gap;
            temp.x = areaC.x - M * 0.5 * latticeW + i * latticeW + gap_offset;
            temp.y = areaC.y - N * 0.5 * latticeH + j * latticeH;
            temp.width = latticeW;
            temp.height = latticeH;
            latticeRect[i].push_back(temp);  //i行にtemp追加
        }
    }

    //検出対象色のパラメータを調整
    int hVal[3] = {5, 30, 108};  // 青色のH値をさらに調整
    int sVal[3] = {40, 40, 60};  // 青色のS値を下げて、より広い範囲を検出
    int vVal[3] = {200, 200, 160};  // 青色のV値をさらに下げる
    int hRange = 15;  // 色相の範囲をさらに広げる
    int sRange = 50;
    int vRange = 90;  // 明度の範囲をさらに広げる

    int hBorder[3][4] = {{0,0,0,0},{0,0,0,0},{0,0,0,0}};
    int sBorder[3][2] = {{0,0},{0,0},{0,0}};
    int vBorder[3][2] = {{0,0},{0,0},{0,0}};

    for (int i=0; i<3; i++) {
        hBorder[i][0] = hVal[i] - hRange;
        if (hBorder[i][0] < 0) {
            hBorder[i][0] = 0;
            hBorder[i][2] = hVal[i] - hRange + 180;
            hBorder[i][3] = 180;
        }
        hBorder[i][1] = hVal[i] + hRange;
        if (hBorder[i][1] > 180) {
            hBorder[i][1] = 180;
            hBorder[i][2] = 0;
            hBorder[i][3] = hVal[i] + hRange - 180;
        }
        //sBorder[i][0] = std::max(0, sVal[i] - sRange);
        //sBorder[i][1] = std::min(255, sVal[i] + sRange);
        sBorder[i][0] = sVal[i];
        sBorder[i][1] = 255;
        vBorder[i][0] = std::max(0, vVal[i] - vRange);
        vBorder[i][1] = std::min(255, vVal[i] + vRange);
    }
    //色範囲確認用のターミナル上の出力
    for (int i=0; i<3; i++) {
        printf("%d: H(%d ~ %d, %d ~ %d), S(%d ~ %d), V(%d ~ %d)\n", i, hBorder[i][0], hBorder[i][1], hBorder[i][2], hBorder[i][3], sBorder[i][0], sBorder[i][1], vBorder[i][0], vBorder[i][1]);
    }

    // 判定済みの格子を記録
    std::vector<std::vector<bool>> checked(M, std::vector<bool>(N, false));
    std::vector<std::vector<int>> changeAmount(M, std::vector<int>(N, 0));

    //抽出領域確認用
    cv::Vec3b rgbCol[3];
    rgbCol[0][0] = 255; rgbCol[0][1] = 0; rgbCol[0][2] = 0;
    rgbCol[1][0] = 0; rgbCol[1][1] = 255; rgbCol[1][2] = 0;
    rgbCol[2][0] = 0; rgbCol[2][1] = 0; rgbCol[2][2] = 255;

    // 色検出の閾値を調整
    int maxChange = 100;  // 検出閾値を150から100に下げる

    //動画像処理無限ループ：「ビデオキャプチャから1フレーム取り込み」→「画像処理」→「表示」の繰り返し
    while (1) {
        //ビデオキャプチャ"capture"から1フレームを取り込んで，"originalImage"に格納
        capture >> originalImage;

        if (originalImage.empty()) { // フレームを取得できなかった場合、ループを出る
            std::cerr << "Error: Could not grab frame." << std::endl;
            break;
        }

        //ビデオが終了したら無限ループから脱出
        if (originalImage.data==NULL) break;

        //メディアンフィルタ適用
        cv::medianBlur(originalImage, originalImage, 3);

        // 縮小処理
        cv::resize(originalImage, camImage, camImage.size());

        // ガンマ補正の適用
        cv::Mat gammaCorrected;
        cv::Mat lookupTable(1, 256, CV_8U);
        uchar* p = lookupTable.ptr();
        for (int i = 0; i < 256; ++i)
            p[i] = cv::saturate_cast<uchar>(pow(i / 255.0, 0.4) * 255.0);
        cv::LUT(camImage, lookupTable, gammaCorrected);

        //"camImage"をHSVに変換して"hsvImage"に格納
        cv::cvtColor(gammaCorrected, hsvImage, cv::COLOR_BGR2HSV);

        // 色領域抽出と格子内の色判定
        int maxColorIndex = -1, maxI = -1, maxJ = -1;
        for (int j = 0; j < N; j++) {
            for (int i = 0; i < M; i++) {
                if (checked[i][j]) continue;  // すでに検出済みの格子は無視

                int colorPixelCount[3] = {0, 0, 0}; // 色ごとのピクセル数を初期化

                for (int y = latticeRect[i][j].y; y < latticeRect[i][j].y + latticeRect[i][j].height; y++) { // すべてのピクセルをスキャン
                    for (int x = latticeRect[i][j].x; x < latticeRect[i][j].x + latticeRect[i][j].width; x++) {
                        cv::Vec3b pixelHSV = hsvImage.at<cv::Vec3b>(y, x); // HSV値を取得

                        //3つの色の範囲内にあるかどうかをチェック
                        for (int k = 0; k < 3; k++) {
                            if ((pixelHSV[0] >= hBorder[k][0] && pixelHSV[0] < hBorder[k][1]) ||
                                (hBorder[k][3] != 0 && pixelHSV[0] >= hBorder[k][2] && pixelHSV[0] < hBorder[k][3])) {
                                if (pixelHSV[1] >= sBorder[k][0] && pixelHSV[1] <= sBorder[k][1] &&
                                    pixelHSV[2] >= vBorder[k][0] && pixelHSV[2] <= vBorder[k][1]) {
                                    colorPixelCount[k]++;
                                }
                            }
                        }
                    }
                }

                int maxChange1 = maxChange;
                maxColorIndex = -1;
                for (int k=0; k<3; k++) {
                    if (colorPixelCount[k]>maxChange1) {
                        maxColorIndex = k;
                        maxChange1 = colorPixelCount[k];
                    }
                }

                //if (i==M/2 && j==N/2) {
                //    printf("%d, %d, %d, %d\n", colorPixelCount[0], colorPixelCount[1], colorPixelCount[2], maxColorIndex);
                //}

                if (maxColorIndex>-1) {
                    maxChange = colorPixelCount[maxColorIndex];
                    maxI = i;
                    maxJ = j;
                    printf("%d, %d\n", maxI, maxJ);
                    break;
                }

                /*
                // 最も多いピクセル数を持つ色を選定
                int maxColorCount = colorPixelCount[0];
                for (int k = 1; k < 3; k++) {
                    if (colorPixelCount[k] > maxColorCount) {
                        maxColorCount = colorPixelCount[k];
                        maxColorIndex = k;
                    }
                }

                // ピクセル数が最大の色が検出された場合
                if (maxColorCount > maxChange) {
                    maxChange = maxColorCount;
                    maxI = i;
                    maxJ = j;
                }
                */
            }

            if (maxColorIndex>-1)
                break;
        }

        int num = 0; //Unityに送信する値
        // 判定された色を格納
        if (maxI != -1 && maxJ != -1) {
            std::string detectedColor = "None"; //色情報
            std::string position = "None"; //位置情報

            if (maxChange > 0) {
                // 色インデックスに基づいて色情報を設定
                if (maxColorIndex == 0) detectedColor = "Red";
                if (maxColorIndex == 1) detectedColor = "Yellow";
                if (maxColorIndex == 2) detectedColor = "Blue";

                //printf("%d\n", maxColorIndex);

                //位置情報を格納
                if (maxI >= 0 && maxI <= 2) {
                    position = "left";
                } else if (maxI >= 3 && maxI <= 5) {
                    position = "center";
                } else if (maxI >= 6 && maxI <= 8) {
                    position = "right";
                }

                // int型に変換してUnityに送信
                if (detectedColor == "Red") {
                    if (position == "left") {
                        num = 11;
                    } else if (position == "center") {
                        num = 21;
                    } else if (position == "right") {
                        num = 31;
                    }
                } else if (detectedColor == "Yellow") {
                    if (position == "left") {
                        num = 12;
                    } else if (position == "center") {
                        num = 22;
                    } else if (position == "right") {
                        num = 32;
                    }
                } else if (detectedColor == "Blue") {
                    if (position == "left") {
                        num = 13;
                    } else if (position == "center") {
                        num = 23;
                    } else if (position == "right") {
                        num = 33;
                    }
                }

                if (num != 0) {
                    //動作確認用処理↓↓
                    // ファイルに書き込み
                    std::ofstream ofs(FILE_NAME, std::ios::trunc);
                    if (!ofs) {
                        std::cerr << "Error: Could not open file for writing." << std::endl;
                        return -1;
                    }
                    ofs << num << std::endl; // 取得した運勢を書き込む
                    ofs.close(); // ファイルをクローズする

                    // 検出結果を画面上に表示
                    cv::putText(camImage, detectedColor, cv::Point(latticeRect[maxI][maxJ].x + 5, latticeRect[maxI][maxJ].y + 20),
                        cv::FONT_HERSHEY_SIMPLEX, 0.5, cv::Scalar(255, 255, 255), 1);

                    checked[maxI][maxJ] = true;  // この格子を判定済みに設定

                    // 検出された格子をハイライト
                    cv::rectangle(camImage, latticeRect[maxI][maxJ], cv::Scalar(255, 255, 255), 2);

                    cv::imshow("Cam", camImage);
                    cv::waitKey(500); // 0.5秒間保持（確認のため）

                    std::ofstream ofs_clear(FILE_NAME, std::ios::trunc); // ファイルの中身を消す
                    if (!ofs_clear) {
                        std::cerr << "Error: Could not open file for clearing." << std::endl;
                        return -1;
                    }
                    ofs_clear.close();
                    //↑↑ここまで動作確認用
                }
            }
        }

        //unityに送信
        sendto(sock, &num, sizeof(num), 0, (struct sockaddr*)&addr, sizeof(addr));

        // 格子を表示
        for (int i = 0; i < M; i++) {
            for (int j = 0; j < N; j++) {
                // 判定済みの格子に色をつける（黄色で塗りつぶし）
                if (checked[i][j]) {
                    cv::rectangle(camImage, latticeRect[i][j], cv::Scalar(255, 255, 255), -1);
                }
                cv::rectangle(camImage, latticeRect[i][j], cv::Scalar(128, 128, 128), 1);
            }
        }

        //ウィンドウに画像表示
        cv::imshow("Cam", camImage);
        /*
        cv::Mat hsvImage2 = hsvImage.clone();
        for (int j=0; j<hsvImage2.rows; j++) {
            for (int i=0; i<hsvImage2.cols; i++) {
                cv::Vec3b s = hsvImage2.at<cv::Vec3b>(j,i);

                if (s[0]>111 && s[0]<114)
                    s[0] = 255;
                else
                    s[0] = 0;


                s[1] = s[2] = s[0];
                hsvImage2.at<cv::Vec3b>(j,i) = s;
            }
        }
        cv::imshow("Cam2", hsvImage2);
        */

        // ユーザーが[r]キーを押したら全ての格子のチェックをリセット
        int key = cv::waitKey(1);
        if (key == 'r') {
            std::fill(checked.begin(), checked.end(), std::vector<bool>(N, false));
            std::fill(changeAmount.begin(), changeAmount.end(), std::vector<int>(N, 0));
            std::cout << "Grid reset" << std::endl;
        }

        //[q]キーが押されたら無限ループから脱出
        if (key=='q') break;
    }

    //メッセージを出力して終了
    printf("Finished\n");

    //終了処理
    close(sock);
    capture.release();
    cv::destroyAllWindows();

    return 0;
}
