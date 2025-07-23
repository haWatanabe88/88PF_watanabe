#include "PlayerManager.h"
#include "GameContext.h"

bool PlayerManager::getPressed_E(){
	return pressed_E;
}

void PlayerManager::setPressed_E(bool rlt){
	pressed_E = rlt;
}

void PlayerManager::handlePlayerInput(GameContext& context){
	if(context.spManager.getCurrentState() == SPManager::SPState::Idle
	|| context.spManager.getCurrentState() == SPManager::SPState::Awake)
	{
		context.playerManager.InputDirection(context.player);//方向キー入力
		context.playerManager.InputAction(context.player, context.enemies, context.spManager, context.blackOut);//必要に応じて攻撃
		context.playerManager.resetPressedE();//Eキーのリセット担当
	}
}

void PlayerManager::InputDirection(Player& p){
	if(!pressed_E){
		if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up)){
			p.setDirection(Player::Direction::up);
		}
		else if(sf::Keyboard::isKeyPressed(sf::Keyboard::Left)){
			p.setDirection(Player::Direction::left);
		}
		else if(sf::Keyboard::isKeyPressed(sf::Keyboard::Down)){
			p.setDirection(Player::Direction::down);
		}
		else if(sf::Keyboard::isKeyPressed(sf::Keyboard::Right)){
			p.setDirection(Player::Direction::right);
		}
		p.resetDistance();//方向が変わるたびにdistanceを初期化する
	}
}

void PlayerManager::InputAction(Player& p, std::vector<Enemy*>& chars, SPManager& spm, BlackOut& blackOut){
	updateCoolTime(spm);
	if (sf::Keyboard::isKeyPressed(sf::Keyboard::E) && !pressed_E){
		if(keyCoolDownTime.getElapsedTime().asSeconds() > coolDownTime){
			pressed_E = true;
			keyCoolDownTime.restart();
			std::cout << "攻撃！" << std::endl;
			p.attack(chars, spm);
		}
	}
	if (spm.getCurrentState() == SPManager::SPState::Idle && SPGauge::getIsGaugeMax() && sf::Keyboard::isKeyPressed(sf::Keyboard::Space)){
			std::cout << "必殺技" << std::endl;
			//この下で必殺技の判定をする
			SPGauge::resetSumGauge();
			blackOut.activeSPattack(spm);
	}
}

void PlayerManager::resetPressedE(){
	if(!pressed_E){
		tmp.restart();
	}
	if(pressed_E && tmp.getElapsedTime().asSeconds() > attackSpriteTime){
		pressed_E = false;
		tmp.restart();
	}
}

void PlayerManager::updateCoolTime(SPManager& spm){
	if(spm.getCurrentState() == SPManager::SPState::Awake){
		coolDownTime = 0.15f;
		attackSpriteTime = 0.125f;
	}else{
		coolDownTime = 0.3f;
		attackSpriteTime = 0.25f;
	}
}

