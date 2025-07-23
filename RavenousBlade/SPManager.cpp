#include "GameContext.h"

void SPManager::checkSPRelease(GameContext& context){
    //////awake中のチェック・・・必殺技関連v
	if(context.spManager.getCurrentState() == SPManager::SPState::Awake){
		int deadCount = std::count_if(context.enemies.begin(), context.enemies.end(), [](Enemy* e){
			return e && e->getIsDead();
		});
		if(deadCount == context.enemies.size()){
			context.spManager.setCurrentState(SPManager::SPState::Finishing);
		}
	}
}

void SPManager::reset(){
	currentState = SPState::Idle;
}