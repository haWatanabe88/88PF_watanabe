#include "ScoreManager.h"

int ScoreManager::score = 0;


void ScoreManager::addScore(int value){
	score += value;
}

void ScoreManager::changeScore(sf::Window& window){
	text.setString("Score : " + std::to_string(score));
	customTextPos(text, window, 20.0f, 0.0f);
}

void ScoreManager::reset(){
	score = 0;
}