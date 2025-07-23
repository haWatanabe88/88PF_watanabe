#pragma once
#include "Enemy.h"

class NormalEnemy : public Enemy{
	public:
		NormalEnemy(Direction dir):Enemy(dir){
			moveSpeed = 100.0f;
			score = 100;
		}
	void setEnemyTexture(Enemy&, EnemyManager&) override;
};