cmake_minimum_required(VERSION 3.10)
project(SFML_App)

set(CMAKE_CXX_STANDARD 17)

# SFML_DIR は Homebrew でインストールした SFML の CMake モジュールがあるディレクトリを指定
# 例: /usr/local/Cellar/sfml@2/2.6.2_1/lib/cmake/SFML
set(SFML_DIR "/usr/local/Cellar/sfml@2/2.6.2_1/lib/cmake/SFML")

find_package(SFML 2 COMPONENTS graphics window system audio network REQUIRED)

add_executable(sfml-app TTT.cpp)
target_link_libraries(sfml-app sfml-graphics sfml-window sfml-system sfml-audio sfml-network)
