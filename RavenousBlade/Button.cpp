#include "Button.h"
#include <iostream>
Button::Button(float x, float y, float width, float height, const std::string& label, sf::Color color, std::function<void()> onClick)
:onClick(onClick)
{
	// ボタンの外観（矩形）
	buttonShape.setSize(sf::Vector2f(width, height));
	buttonShape.setOrigin(buttonShape.getLocalBounds().width / 2.0f, buttonShape.getLocalBounds().height / 2.0f);
	buttonShape.setPosition(x, y);
	buttonShape.setFillColor(sf::Color::Transparent);  //初期色は緑

	// ボタンのテキスト
	font.loadFromFile("./Font/ManufacturingConsent-Regular.ttf");
	buttonText.setFont(font);
	buttonText.setString(label);
	buttonText.setCharacterSize(50);
	buttonText.setOrigin(buttonText.getLocalBounds().width / 2.0f, buttonText.getLocalBounds().height / 2.0f);
	buttonText.setFillColor(color);
	buttonText.setPosition(x,y - buttonText.getLocalBounds().top);
}

bool Button::isClicked(const sf::Vector2i& mousePos) {
	return buttonShape.getGlobalBounds().contains(static_cast<sf::Vector2f>(mousePos));
}

void Button::update(const sf::Vector2i& mousePos, bool mousePressed) {
	// マウスがボタン上に乗った場合
	if (buttonShape.getGlobalBounds().contains(static_cast<sf::Vector2f>(mousePos))) {
		buttonShape.setFillColor(hoverColor);  // ホバー時の色
		if (mousePressed && isClicked(mousePos)) {
			buttonShape.setFillColor(clickColor);  // クリック時の色
			if (onClick) onClick();
		}
	} else {
		buttonShape.setFillColor(sf::Color::Transparent);  // 初期色
	}
}

void Button::draw(sf::RenderWindow& window) {
	window.draw(buttonShape);
	window.draw(buttonText);
}

