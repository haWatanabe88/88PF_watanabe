#pragma once
#include "Enemy.h"

class FastEnemy : public Enemy{
	public:
		FastEnemy(Direction dir):Enemy(dir){
			moveSpeed = 250.0f;
			score = 200;
		}
	void setEnemyTexture(Enemy&, EnemyManager&) override;
};