#pragma once
#include "Enemy.h"

class SlowEnemy : public Enemy{
	public:
		SlowEnemy(Direction dir):Enemy(dir){
			moveSpeed = 50.0f;
			score = 80;
		}
	void setEnemyTexture(Enemy&, EnemyManager&) override;
};