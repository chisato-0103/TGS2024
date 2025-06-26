#include <iostream>
#include <opencv2/opencv.hpp>

int main() {
    std::cout << "利用可能なカメラデバイスを検索中..." << std::endl;

    for (int i = 0; i < 10; i++) {
        cv::VideoCapture cap(i);

        if (cap.isOpened()) {
            std::cout << "カメラ " << i << ": 利用可能" << std::endl;

            // カメラの詳細情報を取得
            double width = cap.get(cv::CAP_PROP_FRAME_WIDTH);
            double height = cap.get(cv::CAP_PROP_FRAME_HEIGHT);
            double fps = cap.get(cv::CAP_PROP_FPS);

            std::cout << "  解像度: " << width << "x" << height << std::endl;
            std::cout << "  FPS: " << fps << std::endl;

            // テストフレームを取得
            cv::Mat frame;
            if (cap.read(frame) && !frame.empty()) {
                std::cout << "  フレーム取得: 成功" << std::endl;
            } else {
                std::cout << "  フレーム取得: 失敗" << std::endl;
            }

            cap.release();
        } else {
            std::cout << "カメラ " << i << ": 利用不可" << std::endl;
        }
    }

    return 0;
}
