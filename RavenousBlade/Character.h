#pragma once
#include <SFML/Graphics.hpp>

class Character{
	public:
		Character() = default;
		virtual ~Character() = default;
		enum class Direction{
			up,
			left,
			down,
			right
		};
		Direction getDirection();
		void setDirection(Direction);
		void showSprite(sf::RenderWindow&);
		void setScale(float);
		bool getIsDead();
		void  setIsDead(bool);
		sf::Vector2f getCenterPos(sf::RenderWindow&);
		sf::Vector2f getPosition();
		sf::Sprite getCharacterSprite(){return characterSprite;};
	protected:
		Direction direction;
		bool isDead = false;
		sf::Texture texture_front;
		sf::Texture texture_left;
		sf::Texture texture_back;
		sf::Texture texture_right;
		sf::Texture texture_front_attack;
		sf::Texture texture_left_attack;
		sf::Texture texture_back_attack;
		sf::Texture texture_right_attack;
		sf::Sprite characterSprite;
};