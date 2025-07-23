#include "GameTitleContext.h"

GameTitleContext::GameTitleContext(sf::RenderWindow& window, GameContext& context)
:startBtn(window.getSize().x / 2.0f
		,window.getSize().y - 300.0f
		,180.0f,60.0f
		,"Start"
		,sf::Color::Green
		,[&context](){context.currentScene = GameContext::SceneState::Playing;})
{
	setFont("./Font/ManufacturingConsent-Regular.ttf");
	titleText.setString("Ravenous Blade");
	setSize(titleText, 120);
	titleText.setOrigin(
		titleText.getLocalBounds().width / 2.0f,
		titleText.getLocalBounds().height / 2.0f
	);
	titleText.setPosition(
		window.getSize().x / 2.0f,
		window.getSize().y / 3.0f
	);
}

bool GameTitleContext::setFont(std::string fontName){
	if (!font.loadFromFile(fontName)){
        return -1; // フォントロード失敗
    }
	titleText.setFont(font);
	return true;
}

void GameTitleContext::setSize(sf::Text& text, int value){
	text.setCharacterSize(value);
}

void GameTitleContext::draw(sf::RenderWindow& window){
	window.clear();
	window.draw(titleText);
	startBtn.draw(window);
	window.display();
}

void GameTitleContext::update(sf::RenderWindow& window){
	sf::Vector2i mousePos = sf::Mouse::getPosition(window);
	bool mousePressed = sf::Mouse::isButtonPressed(sf::Mouse::Left);
	this->startBtn.update(mousePos, mousePressed);
}