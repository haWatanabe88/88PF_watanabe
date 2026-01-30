// Constants.h
#pragma once

// --- ステータスの上限・下限定義（レベルデザイン用） ---
//メリット
const float MOVE_SPEED_MAX = 0.08f;
const float CLEAN_SPEED_MIN = 0.2f;
const int MAX_HP_MAX = 300;
const int ATTACK_POWER_MAX = 100;
const float ATTACK_SPEED_MIN = 0.3f;
const float REVIVAL_TIME_MIN = 0.1f;

//デメリット
const float MOVE_SPEED_MIN = 0.001f;
const float CLEAN_SPEED_MAX = 4.0f;
const int MAX_HP_MIN = 10;
const int ATTACK_POWER_MIN = 1;
const float ATTACK_SPEED_MAX = 5.0f;
const float REVIVAL_TIME_MAX = 10.0f;

// --- ステータス変更量（後で調整しやすい） ---
//メリット
const float MOVE_SPEED_UP = 0.01f;
const float CLEAN_SPEED_UP = -0.5f;
const int MAX_HP_UP = 50;
const int ATTACK_POWER_UP = 15;
const float ATTACK_SPEED_UP = -0.5f;
const float REVIVAL_TIME_DOWN = -1.0f;

//デメリット
const float MOVE_SPEED_DOWN = -0.003f;
const float CLEAN_SPEED_DOWN = 0.2f;
const int MAX_HP_DOWN = -20;
const int ATTACK_POWER_DOWN = -2;
const float ATTACK_SPEED_DOWN = 0.3f;
const float REVIVAL_TIME_UP = 0.3f;

//最大経験値
const float maxExperience = 100.0f;