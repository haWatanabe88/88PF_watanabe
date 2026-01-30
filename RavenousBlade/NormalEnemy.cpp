#include "NormalEnemy.h"
#include "EnemyManager.h"

void NormalEnemy::setEnemyTexture(Enemy& e, EnemyManager& em){
	Direction curDir = e.getDirection();
	if(curDir == Direction::up){
		characterSprite.setTexture(em.getNormalTextureBack());
		if(isDead){
			characterSprite.setTexture(em.getNormalTextureBackAttack());
		}
	}
	else if(curDir == Direction::left){
		characterSprite.setTexture(em.getNormalTextureLeft());
		if(isDead){
			characterSprite.setTexture(em.getNormalTextureLeftAttack());
		}
	}
	else if(curDir == Direction::down){
		characterSprite.setTexture(em.getNormalTextureFront());
		if(isDead){
			characterSprite.setTexture(em.getNormalTextureFrontAttack());
		}
	}
	else if(curDir == Direction::right){
		characterSprite.setTexture(em.getNormalTextureRight());
		if(isDead){
			characterSprite.setTexture(em.getNormalTextureRightAttack());
		}
	}
}