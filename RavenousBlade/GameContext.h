#pragma once
#include <SFML/Graphics.hpp>
#include "Player.h"
#include "PlayerManager.h"
#include "NormalEnemy.h"
#include "FastEnemy.h"
#include "SlowEnemy.h"
#include "EnemyManager.h"
#include "AttackArea.h"
#include "BigAttackArea.h"
#include "SmallAttackArea.h"
#include "ScoreManager.h"
#include "ComboManager.h"
#include "SPGauge.h"
#include "BlackOut.h"
#include "SPManager.h"
#include <iostream>
#include <algorithm>

struct GameContext
{
	sf::RenderWindow window;
	std::vector<AttackArea*> circles;
	BigAttackArea bigCircle;
	SmallAttackArea smallCircle;
	PlayerManager playerManager;
	std::vector<Enemy*> enemies;
	SPManager spManager;
	Player player;
	EnemyManager enemyManager;
	Enemy* spawned;
	sf::RectangleShape outline;
	ScoreManager scoreManager;
	ComboManager comboManager;
	BlackOut blackOut;
	SPGauge spGauge;
	sf::Clock progressClock;
	sf::Clock timerClock;  // 時間管理用


	sf::Text timerText;
	sf::Font font;
	float timer;

	enum class SceneState{
		Title,
		Playing,
		GameOver,
	};
	SceneState currentScene = SceneState::Title;

	GameContext();
	void settingWindow();
	void draw();
	void reset(GameContext&);//初期化関数
	void updateTimer();
	void resetTimer();
};

