#include "ComboManager.h"

int  ComboManager::comboCounter = 0;
float ComboManager::comboMultiplier = 1.0f;

void ComboManager::changeComboRank(int comboCounter){
	if(comboCounter >= 5){
		comboRank = ComboRank::genius;
		text.setString("Genius!!");
	}else if(comboCounter >= 4){
		comboRank = ComboRank::marvelous;
		text.setString("Marvelous!");
	}else if(comboCounter >= 3){
		comboRank = ComboRank::wonderful;
		text.setString("Wonderful");
	}else if(comboCounter >= 2){
		comboRank = ComboRank::great;
		text.setString("Great");
	}else if(comboCounter >= 1){
		comboRank = ComboRank::good;
		text.setString("Good");
	}else{
		comboRank = ComboRank::normal;
		text.setString("");
	}
	changeComboMultiplier(comboRank);
}

void ComboManager::changeComboMultiplier(ComboRank comboRank){
	switch (comboRank)
	{
	case ComboRank::normal:
		comboMultiplier = 1.0f;
		break;
	case ComboRank::good:
		comboMultiplier = 1.2f;
		break;
	case ComboRank::great:
		comboMultiplier = 1.5f;
		break;
	case ComboRank::wonderful:
		comboMultiplier = 2.0f;
		break;
	case ComboRank::marvelous:
		comboMultiplier = 3.0f;
		break;
	case ComboRank::genius:
		comboMultiplier = 5.0f;
		break;
	default:
		break;
	}
}

void ComboManager::changeComboGrade(sf::Window& window, int comboCounter){
	changeComboRank(comboCounter);
	customTextPos(text, window, 20.0f, 70.0f);
}

void ComboManager::changeComboCounter(sf::Window& window){
	if(comboCounter >= 2){
		text2.setString(std::to_string(comboCounter) + " combo");
	}else{
		text2.setString("");
	}
	customTextPos(text2, window, 20.0f, 130.0f);
}

void ComboManager::reset(){
	comboCounter = 0;
}