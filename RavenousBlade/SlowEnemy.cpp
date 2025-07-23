#include "SlowEnemy.h"
#include "EnemyManager.h"

void SlowEnemy::setEnemyTexture(Enemy& e, EnemyManager& em){
	Direction curDir = e.getDirection();
	if(curDir == Direction::up){
		characterSprite.setTexture(em.getSlowTextureBack());
		if(isDead){
			characterSprite.setTexture(em.getSlowTextureBackAttack());
		}
	}
	else if(curDir == Direction::left){
		characterSprite.setTexture(em.getSlowTextureLeft());
		if(isDead){
			characterSprite.setTexture(em.getSlowTextureLeftAttack());
		}
	}
	else if(curDir == Direction::down){
		characterSprite.setTexture(em.getSlowTextureFront());
		if(isDead){
			characterSprite.setTexture(em.getSlowTextureFrontAttack());
		}
	}
	else if(curDir == Direction::right){
		characterSprite.setTexture(em.getSlowTextureRight());
		if(isDead){
			characterSprite.setTexture(em.getSlowTextureRightAttack());
		}
	}
}