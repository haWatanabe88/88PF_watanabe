#include "BlackOut.h"

BlackOut::BlackOut(sf::RenderWindow& window) {
		sf::Vector2u windowSize = window.getSize();
		maxRadius = std::sqrt(windowSize.x * windowSize.x + windowSize.y * windowSize.y) * 0.7f;
		centerPos = sf::Vector2f(windowSize.x / 2.0f, windowSize.y / 2.0f);
		// マスク用テクスチャ生成
		maskTexture.create(windowSize.x, windowSize.y);
		// 黒のオーバーレイ
		fullOverlay.setSize(sf::Vector2f(windowSize));
		fullOverlay.setFillColor(sf::Color(0, 0, 0, 0));
		// 初期円（透明のくり抜き部分）
		clearCircle.setRadius(currentRadius);
		clearCircle.setOrigin(currentRadius, currentRadius);
		clearCircle.setPosition(centerPos);
		clearCircle.setFillColor(sf::Color::Transparent);
		if (!awakeBuffer.loadFromFile("./Sound/AwakeSoundSE.wav")) {
			std::cerr << "Failed to load awakeBuffer.wav" << std::endl;
		} else {
			awakeSound.setBuffer(awakeBuffer);
		}
}

void BlackOut::activeSPattack(SPManager& spState){
		spState.setCurrentState(SPManager::SPState::TimeStopStart);
		this->startClearing(spState);
}

void BlackOut::startClearing(SPManager& spState){
	fullOverlay.setFillColor(sf::Color(0, 0, 0, alpha));
	currentRadius = 100.0f;
}

void BlackOut::circleSizeUpAnim(SPManager& spState){
	if(spState.getCurrentState() == SPManager::SPState::BlinkEnd){
		if(currentRadius < maxRadius){
			if(!hasPlayedAwakeSound){
				hasPlayedAwakeSound = true;
				awakeSound.play();
			}
			std::cout << currentRadius << std::endl;
			currentRadius += 0.1f; // アニメーション速度
			if(currentRadius >= maxRadius){
				hasPlayedAwakeSound = false;
				currentRadius = maxRadius;
				spState.setCurrentState(SPManager::SPState::Awake);//暗転が開き切ったということの通知
				std::cout << "Awake" << std::endl;
			}
		}
	}
	clearCircle.setRadius(currentRadius);
	clearCircle.setOrigin(currentRadius, currentRadius);
	clearCircle.setPosition(centerPos);
}

void BlackOut::draw(sf::RenderWindow& window, SPManager& spState){
	this->circleSizeUpAnim(spState);
	maskTexture.clear(sf::Color::Transparent);
	// 黒幕描画
	maskTexture.draw(fullOverlay);
	// 穴あき部分を「透明のBlendModeで」描画
	sf::RenderStates states;
	states.blendMode = sf::BlendNone; // 透明部分で塗りつぶす
	maskTexture.draw(clearCircle, states);
	// 結果をウィンドウに描画
	maskTexture.display();
	maskSprite.setTexture(maskTexture.getTexture());
	window.draw(maskSprite);
}