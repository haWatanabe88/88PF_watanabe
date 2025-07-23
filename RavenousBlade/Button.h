#pragma once
#include <SFML/Graphics.hpp>

class Button{
	private:
		sf::RectangleShape buttonShape;
		sf::Text buttonText;
		sf::Font font;
		sf::Color hoverColor = sf::Color::Yellow;  // ホバー時の色
		sf::Color clickColor = sf::Color::Red;     // クリック時の色
		std::function<void()> onClick;
	public:
		Button(float x, float y, float width, float height, const std::string& label, sf::Color color, std::function<void()> onClick);
		void setHoverColor(sf::Color color){hoverColor = color;};
		void setClickColor(sf::Color color){clickColor = color;};
		bool isClicked(const sf::Vector2i&);
		void update(const sf::Vector2i&, bool);
		void draw(sf::RenderWindow&);
};