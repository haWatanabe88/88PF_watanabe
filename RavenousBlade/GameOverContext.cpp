#include "GameOverContext.h"

GameOverContext::GameOverContext(sf::RenderWindow& window, GameContext& context)
	:titleBtn(window.getSize().x / 2.0f, window.getSize().y - 200.0f, 180.0f, 60.0f, "Exit", sf::Color::Green, [&context](){context.currentScene = GameContext::SceneState::Title;})
	,gauge(window.getSize().x / 2.0f, 450, 300, 20, 10000, window) // ← 位置・幅・高さ・1ランクあたりのスコア
	{
		sf::Vector2u windowSize = window.getSize();
		setFont("./Font/ManufacturingConsent-Regular.ttf");
		gauge.loadRankTextures({
			"./image/bronze.png",
			"./image/silver.png",
			"./image/gold.png",
			"./image/platinum.png",
			"./image/double_S.png",
			"./image/triple_S.png",
		});
		gameOverText.setString("Game Over!");
		setSize(gameOverText, 120);
		setSize(scoreText, 150);
		gameOverText.setOrigin(gameOverText.getLocalBounds().width / 2.0f
								,gameOverText.getLocalBounds().height / 2.0f);
		scoreText.setOrigin(scoreText.getLocalBounds().width / 2.0f
						,scoreText.getLocalBounds().height / 2.0f);
		gameOverText.setPosition(windowSize.x / 2.0f, 50.0f);
		fullOverlay.setSize(sf::Vector2f(windowSize));
		fullOverlay.setFillColor(sf::Color(0, 0, 0, 10));
		curScore = 0;
		th = 2.0f;
		time = 0;
		curState = GaugeState::normal;
	};

bool GameOverContext::setFont(std::string fontName){
	if (!font.loadFromFile(fontName)){
        return -1; // フォントロード失敗
    }
	gameOverText.setFont(font);
	scoreText.setFont(font);
	return true;
}

void GameOverContext::setSize(sf::Text& text, int value){
	text.setCharacterSize(value);
}

void GameOverContext::draw(sf::RenderWindow& window, int score){
	window.clear();
	window.draw(fullOverlay);
	window.draw(gameOverText);
	scoreUpdate(window, scoreText, score, curScore);
    gauge.draw(window); // 描画追加
	titleBtn.draw(window);
	window.display();
}

void GameOverContext::update(sf::RenderWindow& window){
	sf::Vector2i mousePos = sf::Mouse::getPosition(window);
	bool mousePressed = sf::Mouse::isButtonPressed(sf::Mouse::Left);
	this->titleBtn.update(mousePos, mousePressed); // もしもう一つボタンあるなら
}

void GameOverContext::scoreUpdate(sf::RenderWindow& window, sf::Text scoreText, int score, int& curScore){
	if(curState == GaugeState::normal){
		if(curScore < score)
		{
			curScore++;
			progressClock.restart();
		}
		else{
			curScore = score;
			time += progressClock.restart().asSeconds();
			if(time > th)
			{
				time = th;
				curState = GaugeState::up;
			}
		}
	}
	else if(curState == GaugeState::up){
		if(curScore >= 0)
		{
			curScore--;
			gauge.updateScore(1); // ← スコア1ずつ加算に合わせてゲージ更新
		}
		else{
			curScore = 0;
			curState = GaugeState::down;
		}
	}
	scoreText.setString(std::to_string(curScore));
	scoreText.setOrigin(scoreText.getLocalBounds().width / 2.0f
				,scoreText.getLocalBounds().height / 2.0f);
	scoreText.setPosition(window.getSize().x / 2.0f, 300.0f);
	window.draw(scoreText);
}

void GameOverContext::reset(){
	curScore = 0;
	time = 0;
	gauge.reset();
	curState = GaugeState::normal;
}