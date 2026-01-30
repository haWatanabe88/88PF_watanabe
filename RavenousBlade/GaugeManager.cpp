#include "GaugeManager.h"

GaugeManager::GaugeManager(float x, float y, float gaugeWidth, float gaugeHeight, int maxScorePerRank, sf::RenderWindow& window)
    : x(x), y(y), gaugeWidth(gaugeWidth), gaugeHeight(gaugeHeight),
		score(0), maxScorePerRank(maxScorePerRank), rankIndex(0)
{
    gaugeBackground.setSize(sf::Vector2f(gaugeWidth, gaugeHeight));
	gaugeBackground.setOrigin(gaugeBackground.getLocalBounds().width / 2.0f, gaugeBackground.getLocalBounds().height / 2.0f);
    gaugeBackground.setPosition(x, y);
    gaugeBackground.setFillColor(sf::Color(50, 50, 50));
	gaugeBar.setSize(sf::Vector2f(gaugeWidth, gaugeHeight));
	gaugeBar.setOrigin(gaugeBar.getLocalBounds().width / 2.0f, gaugeBar.getLocalBounds().height / 2.0f);
    gaugeBar.setSize(sf::Vector2f(0, gaugeHeight)); // 初期値はゼロ
    gaugeBar.setPosition(x, y);
    gaugeBar.setFillColor(sf::Color(100, 250, 50));
	rankSprite.setScale(0.4, 0.4);
	rankSprite.setOrigin(rankSprite.getLocalBounds().width/2.0f, rankSprite.getLocalBounds().height/2.0f);
    rankSprite.setPosition(x - 110, y + 50);
}

void GaugeManager::loadRankTextures(const std::vector<std::string>& texturePaths) {
    rankTextures.clear();
    for (const auto& path : texturePaths){
        sf::Texture texture;
        if (texture.loadFromFile(path)){
            rankTextures.push_back(texture);
        }
    }
    updateRankSprite();
}

void GaugeManager::updateScore(int delta) {
    score += delta;
    std::cout << rankTextures.size() << std::endl;
    while (score >= maxScorePerRank && rankIndex + 1 < rankTextures.size()) {
        score -= maxScorePerRank;
        rankIndex++;
        updateRankSprite();
    }
    updateGaugeBar();
}

void GaugeManager::updateGaugeBar() {
    float ratio = static_cast<float>(score) / maxScorePerRank;
	if(score <= maxScorePerRank)
    	gaugeBar.setSize(sf::Vector2f(gaugeWidth * ratio, gaugeHeight));
}

void GaugeManager::updateRankSprite() {
    if (rankIndex < rankTextures.size()) {
        rankSprite.setTexture(rankTextures[rankIndex]);
    }
}

void GaugeManager::draw(sf::RenderWindow& window) {
    window.draw(gaugeBackground);
    window.draw(gaugeBar);
    if (!rankTextures.empty()) {
        window.draw(rankSprite);
    }
}

int GaugeManager::getScore() const {
    return score;
}

int GaugeManager::getRankIndex() const {
    return rankIndex;
}

void GaugeManager::reset() {
    score = 0;
    rankIndex = 0;
    updateGaugeBar();
    updateRankSprite();
}
