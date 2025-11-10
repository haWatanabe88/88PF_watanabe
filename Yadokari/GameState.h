#pragma once

// 画面状態を示す列挙型
enum class GameState {
    Title,  // タイトル画面
    Game,   // ゲームプレイ画面
    Clear,  // ゲームクリア画面
    GameOver // 汚染ゲージ上限到達時の画面（GameOver... 赤文字）
};