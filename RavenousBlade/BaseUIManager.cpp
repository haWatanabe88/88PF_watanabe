#include "BaseUIManager.h"

bool BaseUIManager::setFont(std::string fontName){
	if (!font.loadFromFile(fontName)){
		std::cout << "失敗" << std::endl;
        return -1; // フォントロード失敗
    }
	text.setFont(font);
	text2.setFont(font);
	return true;
}

void BaseUIManager::setSize(int value){
	text.setCharacterSize(value * 1.1f);
	text2.setCharacterSize(value * 0.8f);
}

void BaseUIManager::setColor(sf::Color fontColor){
	text.setFillColor(fontColor);
	text2.setFillColor(fontColor);
}

void BaseUIManager::customTextPos(sf::Text& text, sf::Window& window, float margin_x, float margin_y){
	sf::FloatRect bounds = text.getLocalBounds();
    text.setOrigin(bounds.left + bounds.width, bounds.top);
    text.setPosition(window.getSize().x - margin_x, margin_y);
}