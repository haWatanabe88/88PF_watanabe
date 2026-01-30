#include "FastEnemy.h"
#include "EnemyManager.h"

void FastEnemy::setEnemyTexture(Enemy& e, EnemyManager& em){
	Direction curDir = e.getDirection();
	if(curDir == Direction::up){
		characterSprite.setTexture(em.getFastTextureBack());
		if(isDead){
			characterSprite.setTexture(em.getFastTextureBackAttack());
		}
	}
	else if(curDir == Direction::left){
		characterSprite.setTexture(em.getFastTextureLeft());
		if(isDead){
			characterSprite.setTexture(em.getFastTextureLeftAttack());
		}
	}
	else if(curDir == Direction::down){
		characterSprite.setTexture(em.getFastTextureFront());
		if(isDead){
			characterSprite.setTexture(em.getFastTextureFrontAttack());
		}
	}
	else if(curDir == Direction::right){
		characterSprite.setTexture(em.getFastTextureRight());
		if(isDead){
			characterSprite.setTexture(em.getFastTextureRightAttack());
		}
	}
}