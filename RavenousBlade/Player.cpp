#include "Player.h"
#include "PlayerManager.h"
#include "EnemyManager.h"
#include "GameContext.h"

float Player::getDistance(){
	return distance;
}

void Player::setDistance(float newDistance){
	distance = newDistance;
}

//方向が変わった時
void Player::resetDistance(){
	setDistance(std::numeric_limits<float>::max());
}

//最も近い矩形辺の座標を取得
float Player::calcDistance(Enemy& e){
	sf::Vector2f playerPos = this->getPosition();
	sf::FloatRect enemyBounds = e.getCharacterSprite().getGlobalBounds();
	float closest_X = std::clamp(playerPos.x, enemyBounds.left, enemyBounds.left + enemyBounds.width);
	float closest_Y = std::clamp(playerPos.y, enemyBounds.top, enemyBounds.top + enemyBounds.height);
	float dx = closest_X - playerPos.x;
	float dy = closest_Y- playerPos.y;
	return sqrt(dx*dx + dy*dy);
}

bool Player::isMinimumDistance(float newDistance){
	if(this->getDistance() >= newDistance)
	{
		this->setDistance(newDistance);
		return true;
	}
	return false;
}

bool Player::loadAllPlayerTexture(){
	if(!texture_front.loadFromFile("./image/player_front.png")) return false;
	if(!texture_left.loadFromFile("./image/player_left.png")) return false;
	if(!texture_back.loadFromFile("./image/player_back.png")) return false;
	if(!texture_right.loadFromFile("./image/player_right.png")) return false;
	if(!texture_front_attack.loadFromFile("./image/player_front_attack.png")) return false;
	if(!texture_left_attack.loadFromFile("./image/player_left_attack.png")) return false;
	if(!texture_back_attack.loadFromFile("./image/player_back_attack.png")) return false;
	if(!texture_right_attack.loadFromFile("./image/player_right_attack.png")) return false;
	if(!texture_front_sp.loadFromFile("./image/player_front_sp.png")) return false;
	if(!texture_back_sp.loadFromFile("./image/player_back_sp.png")) return false;
	if(!texture_left_sp.loadFromFile("./image/player_left_sp.png")) return false;
	if(!texture_right_sp.loadFromFile("./image/player_right_sp.png")) return false;
	if(!texture_front_attack_sp.loadFromFile("./image/player_front_attack_sp.png")) return false;
	if(!texture_back_attack_sp.loadFromFile("./image/player_back_attack_sp.png")) return false;
	if(!texture_left_attack_sp.loadFromFile("./image/player_left_attack_sp.png")) return false;
	if(!texture_right_attack_sp.loadFromFile("./image/player_right_attack_sp.png")) return false;
	if(!texture_front_eyeClose.loadFromFile("./image/player_front_eyeClose.png")) return false;
	if(!texture_left_eyeClose.loadFromFile("./image/player_left_eyeClose.png")) return false;
	if(!texture_right_eyeClose.loadFromFile("./image/player_right_eyeClose.png")) return false;
	if(!texture_back_eyeClose.loadFromFile("./image/player_back_eyeClose.png")) return false;
	return true;
}

void Player::setPosinWindowCenter(sf::RenderWindow& window){
    characterSprite.setPosition(getCenterPos(window));
}

void Player::setTexture(PlayerManager& pm, SPManager& spm){
	Direction curDir = this->getDirection();
	if(curDir == Direction::up){
		characterSprite.setTexture(texture_back);
		if(spm.getCurrentState() == SPManager::SPState::TimeStopStart
		|| spm.getCurrentState() == SPManager::SPState::BlinkStart){
			pm.setPressed_E(false);////////////
			blinkAnim(curDir, spm);
		}
		if(spm.getCurrentState() == SPManager::SPState::BlinkEnd
		|| spm.getCurrentState() == SPManager::SPState::Awake)
		{
			characterSprite.setTexture(texture_back_sp);
		}
		if(pm.getPressed_E()){
			characterSprite.setTexture(texture_back_attack);
			if(spm.getCurrentState() == SPManager::SPState::Awake){
				characterSprite.setTexture(texture_back_attack_sp);
			}
		}
	}
	else if(curDir == Direction::left) {
		characterSprite.setTexture(texture_left);
		if(spm.getCurrentState() == SPManager::SPState::TimeStopStart
		|| spm.getCurrentState() == SPManager::SPState::BlinkStart){
			pm.setPressed_E(false);////////////
			blinkAnim(curDir, spm);
		}
		if(spm.getCurrentState() == SPManager::SPState::BlinkEnd
		|| spm.getCurrentState() == SPManager::SPState::Awake)
		{
			characterSprite.setTexture(texture_left_sp);
		}
		if(pm.getPressed_E()){
			characterSprite.setTexture(texture_left_attack);
			if(spm.getCurrentState() == SPManager::SPState::Awake){
				characterSprite.setTexture(texture_left_attack_sp);
			}
		}
	}
	else if(curDir == Direction::down) {
		characterSprite.setTexture(texture_front);
		if(spm.getCurrentState() == SPManager::SPState::TimeStopStart
		|| spm.getCurrentState() == SPManager::SPState::BlinkStart){
			pm.setPressed_E(false);
			blinkAnim(curDir, spm);
		}
		if(spm.getCurrentState() == SPManager::SPState::BlinkEnd
		|| spm.getCurrentState() == SPManager::SPState::Awake)
		{
			characterSprite.setTexture(texture_front_sp);
		}
		if(pm.getPressed_E()){
			characterSprite.setTexture(texture_front_attack);
			if(spm.getCurrentState() == SPManager::SPState::Awake){
				characterSprite.setTexture(texture_front_attack_sp);
			}
		}
	}
	else if(curDir == Direction::right) {
		characterSprite.setTexture(texture_right);
		if(spm.getCurrentState() == SPManager::SPState::TimeStopStart
		|| spm.getCurrentState() == SPManager::SPState::BlinkStart){
			pm.setPressed_E(false);////////////
			blinkAnim(curDir, spm);
		}
		if(spm.getCurrentState() == SPManager::SPState::BlinkEnd
		|| spm.getCurrentState() == SPManager::SPState::Awake)
		{
			characterSprite.setTexture(texture_right_sp);
		}
		if(pm.getPressed_E()){
			characterSprite.setTexture(texture_right_attack);
			if(spm.getCurrentState() == SPManager::SPState::Awake){
				characterSprite.setTexture(texture_right_attack_sp);
			}
		}
	}
}

void Player::blinkAnim(Direction dir, SPManager& spm){
	if(spm.getCurrentState() == SPManager::SPState::TimeStopStart){
		spm.setCurrentState(SPManager::SPState::BlinkStart);
		TimerUtils::reset("blinkClock");
	}
	if(spm.getCurrentState() == SPManager::SPState::BlinkStart){
		if(!TimerUtils::hasElapsed("blinkClock", 2.0f)){
			if(dir == Direction::up){
				characterSprite.setTexture(texture_back_eyeClose);
			}else if(dir == Direction::left){
				characterSprite.setTexture(texture_left_eyeClose);
			}else if(dir == Direction::right){
				characterSprite.setTexture(texture_right_eyeClose);
			}else if(dir == Direction::down){
				characterSprite.setTexture(texture_front_eyeClose);
			}
		}else{
			spm.setCurrentState(SPManager::SPState::BlinkEnd);
		}
	}
}

Enemy* Player::getNowTarget(){
	return nowTarget;
}

void Player::setNowTarget(Enemy* target){
	nowTarget = target;
}

void Player::setTarget(Enemy* e){
	if(this->getDirection() == e->getDirection()){
		if(!e->getIsDead() && isMinimumDistance(this->calcDistance(*e))){
			setNowTarget(e);
		}
	}
}

void Player::rockOnOutLine(sf::RectangleShape& outline){
	if(this->getNowTarget() && this->getNowTarget()->areaType != Enemy::AreaType::None)
	{
		sf::FloatRect bounds = this->getNowTarget()->getCharacterSprite().getGlobalBounds();
		outline.setSize(sf::Vector2f(bounds.width, bounds.height));
		outline.setPosition(bounds.left, bounds.top);
		outline.setFillColor(sf::Color::Transparent);
		if(this->getNowTarget()->areaType == Enemy::AreaType::Big){
			outline.setOutlineColor(sf::Color::Yellow);
			outline.setOutlineThickness(2.f);
		}else if(this->getNowTarget()->areaType == Enemy::AreaType::Small){
			outline.setOutlineColor(sf::Color::Red);
			outline.setOutlineThickness(4.f);
		}
	}else{
		outline.setOutlineThickness(0.f);
	}
}

//エネミー（nowTarget）に対して攻撃する
void Player::attack(std::vector<Enemy*>& chars, SPManager& spm){
	auto it = std::find(chars.begin(), chars.end(), nowTarget);
	if(it != chars.end()){
		if(!((dynamic_cast<Enemy*>(*it))->areaType == Enemy::AreaType::None))
		{
			attackSound.stop();
			criticalAttackSound.stop();
			if((dynamic_cast<Enemy*>(*it))->areaType == Enemy::AreaType::Small)
			{
				criticalAttackSound.play();
			}else{
				attackSound.play();
			}
			(*it)->defeated(spm);
			nowTarget = nullptr;
			resetDistance();
		}
	}
}

bool Player::enemyIsHitted(Enemy& enemy){
	Direction dir = enemy.getDirection();
	if(dir == Direction::up || dir == Direction::down){
		if(calcDistance(enemy) <= halfHight){
			return true;
		}
	}else if(dir == Direction::left || dir == Direction::right){
		if(calcDistance(enemy) <= halfWidth){
			return true;
		}
	}
	return false;
}

void Player::defeated(GameContext& context, Enemy& enemy){
	enemy.setIsDead(true);
	std::cout << "playerやられた" << std::endl;
	if(context.currentScene == GameContext::SceneState::Playing){
		context.currentScene = GameContext::SceneState::GameOver;
	}
}

void Player::reset(GameContext& context){
	setNowTarget(nullptr);
	direction = Direction::down;
	rockOnOutLine(context.outline);
}