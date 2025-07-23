#include "GameContext.h"

GameContext::GameContext()
        : window(sf::VideoMode(1000, 1000), "Ravenous Bladed"),
		bigCircle(200, sf::Color(245, 245, 220, 50), window, circles),
		smallCircle(100, sf::Color(255, 232, 255, 255), window, circles),
		player(window, playerManager, spManager),
		scoreManager("./Font/ManufacturingConsent-Regular.ttf", sf::Color::White, window, 50),
		comboManager("./Font/ManufacturingConsent-Regular.ttf", sf::Color::Blue, window, 50),
		blackOut(window),
		spawned(nullptr)
        {
            timer = 300.0f;
            font.loadFromFile("./Font/ManufacturingConsent-Regular.ttf"); // フォント読み込み（すでにしていれば不要）
            timerText.setCharacterSize(80);
            timerText.setFont(font);
            timerText.setFillColor(sf::Color::White);
            timerText.setString(std::to_string(static_cast<int>(timer)));
            sf::FloatRect textBounds = timerText.getLocalBounds();
            timerText.setOrigin(textBounds.width, textBounds.height);
            timerText.setPosition(window.getSize().x - 50, window.getSize().y - 50); // 右下に余白を持たせて配置
        }

void GameContext::settingWindow(){
	this->window.clear(sf::Color (204, 232, 204, 50));
}

void GameContext::draw(){
	//*描画順は大事なので注意
    //サークルの描画
        for(AttackArea* aa : this->circles){
                aa->showSprite(this->window, this->spManager);
        }
    //キャラクター描画処理

        for(Enemy* enemy : this->enemies){
            enemy->showSprite(this->window);
        }
        this->player.showSprite(this->window);
        this->window.draw(this->outline);
        this->scoreManager.changeScore(this->window);
        this->comboManager.changeComboGrade(this->window, this->comboManager.getComboCounter());
        this->comboManager.changeComboCounter(this->window);
        this->spGauge.updateMaskSprite();
        this->window.draw(this->scoreManager.getText());
        this->window.draw(this->comboManager.getText());
        this->window.draw(this->comboManager.getText2());
        this->window.draw(this->spGauge.getSprite());
        this->window.draw(this->spGauge.getMaskSprite());
        this->blackOut.draw(this->window, this->spManager);
        this->window.display();
}

void GameContext::updateTimer() {
    // 経過時間が1秒を超えていたら更新
    if (timerClock.getElapsedTime().asSeconds() >= 1.0f) {
        if (timer > 0.0f) {
            timer -= 1.0f;
        }
        timerClock.restart();

        // テキストを更新
        timerText.setString(std::to_string(static_cast<int>(timer)));

        // サイズが変わるかもしれないので原点・位置を更新
        sf::FloatRect textBounds = timerText.getLocalBounds();
        timerText.setOrigin(textBounds.width, textBounds.height);
        timerText.setPosition(window.getSize().x - 50, window.getSize().y - 50);
    }
    if(timer <= 0){
        timer = 0;
        if(currentScene == GameContext::SceneState::Playing){
            currentScene = GameContext::SceneState::GameOver;
        }
    }
    // 毎フレーム描画
    window.draw(timerText);
}

void GameContext::resetTimer(){
    timer = 300.0f;
    timerClock.restart();
    // テキストを更新
    timerText.setString(std::to_string(static_cast<int>(timer)));

    // サイズが変わるかもしれないので原点・位置を更新
    sf::FloatRect textBounds = timerText.getLocalBounds();
    timerText.setOrigin(textBounds.width, textBounds.height);
    timerText.setPosition(window.getSize().x - 50, window.getSize().y - 50);
}

void GameContext::reset(GameContext& context){
    resetTimer();
    spGauge.reset();
    comboManager.reset();
    enemyManager.reset(context); // 敵のリストを初期化
    scoreManager.reset();
    spManager.reset();
    player.reset(context);
}