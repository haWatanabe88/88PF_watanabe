#include <SFML/Graphics.hpp>
#include <iostream>
#include <vector>
#include <array>
#include <cstdlib>
#include <ctime>
#include <cmath>
#include <algorithm>
#include <sstream>
#include <iomanip>
#include <map>

// Enumeration for game screen state
enum class GameState {
    Title,
    Game,
    Clear,
    GameOver
};

// Player class encapsulating player stats and behavior
class Player {
public:
    // Player stats
    float moveSpeed;
    float cleanSpeed;
    int hp;
    int maxHp;
    int attackPower;
    float attackSpeed;
    float revivalTime;

    // Sprite and texture for the player
    sf::Sprite sprite;
    sf::Texture texture;
    bool loaded;  // whether the texture is loaded

    // HP bar shapes for player
    sf::RectangleShape hpBarBackground;
    sf::RectangleShape hpBar;

    // Revival state tracking
    bool isReviving;
    sf::Clock revivalClock;
    sf::Vector2f revivalBasePos;
    bool reviveWiggleToggle;

    // Animation state for the player sprite
    int currentFrameIndex;
    sf::Clock animationClock;

    // Constants for player animation frames (sprite sheet parameters)
    static const int FRAME_WIDTH = 235;
    static const int FRAME_HEIGHT = 200;
    static const int NUM_FRAMES = 4;
    const float animationSpeed;  // seconds per frame (0.2f)

    // Static player stat limits (max/min values) and upgrade increments
    static float MOVE_SPEED_MAX;
    static float CLEAN_SPEED_MIN;
    static int   MAX_HP_MAX;
    static int   ATTACK_POWER_MAX;
    static float ATTACK_SPEED_MIN;
    static float REVIVAL_TIME_MIN;
    static float MOVE_SPEED_MIN;
    static float CLEAN_SPEED_MAX;
    static int   MAX_HP_MIN;
    static int   ATTACK_POWER_MIN;
    static float ATTACK_SPEED_MAX;
    static float REVIVAL_TIME_MAX;
    static float MOVE_SPEED_UP;
    static float CLEAN_SPEED_UP;
    static int   MAX_HP_UP;
    static int   ATTACK_POWER_UP;
    static float ATTACK_SPEED_UP;
    static float REVIVAL_TIME_DOWN;
    static float MOVE_SPEED_DOWN;
    static float CLEAN_SPEED_DOWN;
    static int   MAX_HP_DOWN;
    static int   ATTACK_POWER_DOWN;
    static float ATTACK_SPEED_DOWN;
    static float REVIVAL_TIME_UP;

    // Constructor: initialize default stats and HP bar
    Player() 
    : loaded(false), isReviving(false), reviveWiggleToggle(false),
      currentFrameIndex(0), animationSpeed(0.2f) 
    {
        // Initial stats
        moveSpeed = 0.01f;
        cleanSpeed = 2.0f;
        hp = 100;
        maxHp = 100;
        attackPower = 10;
        attackSpeed = 2.0f;
        revivalTime = 5.0f;

        // Set up the HP bar shapes
        hpBarBackground = sf::RectangleShape(sf::Vector2f(64.0f, 8.0f));
        hpBarBackground.setFillColor(sf::Color(100, 100, 100));
        hpBar = sf::RectangleShape(sf::Vector2f(64.0f, 8.0f));
        hpBar.setFillColor(sf::Color::Green);
    }

    // Load the player sprite texture and initialize the sprite (call when game starts)
    bool loadSprite(const std::string& filename) {
        if (!texture.loadFromFile(filename)) {
            std::cerr << "Failed to load " << filename << std::endl;
            return false;
        }
        sprite.setTexture(texture);
        currentFrameIndex = 0;
        // Set initial frame rectangle for the sprite
        sprite.setTextureRect(sf::IntRect(0, 0, FRAME_WIDTH, FRAME_HEIGHT));
        // Center origin and scale down the sprite
        sprite.setOrigin(FRAME_WIDTH / 2.0f, FRAME_HEIGHT / 2.0f);
        sprite.setScale(0.3f, 0.3f);
        // Initial position of the player
        sprite.setPosition(400, 300);
        loaded = true;
        return true;
    }

    // Update the sprite animation frame (if enough time elapsed and player is moving)
    void updateAnimation() {
        if (animationClock.getElapsedTime().asSeconds() >= animationSpeed) {
            currentFrameIndex = (currentFrameIndex + 1) % NUM_FRAMES;
            // Update sprite texture rectangle (offset as in original code)
            sprite.setTextureRect(sf::IntRect(currentFrameIndex * FRAME_WIDTH + 50, 400, FRAME_WIDTH, FRAME_HEIGHT));
            animationClock.restart();
        }
    }

    // Handle revival animation and logic (called when player is in revival state)
    void updateRevival() {
        sf::Vector2f currentScale = sprite.getScale();
        // Flip sprite vertically (upside-down) during revival
        sprite.setScale(currentScale.x, -std::abs(currentScale.y));
        float elapsed = revivalClock.getElapsedTime().asSeconds();
        // Wiggle animation during revival (sprite moves up and down)
        if (std::fmod(elapsed, 0.5f) < 0.01f) {
            float offset = reviveWiggleToggle ? -3.0f : 3.0f;
            sprite.setPosition(revivalBasePos.x, revivalBasePos.y + offset);
            reviveWiggleToggle = !reviveWiggleToggle;
        }
        // If revival time is complete, restore the player's state
        if (elapsed >= revivalTime) {
            hp = maxHp;
            isReviving = false;
            // Restore sprite orientation and position
            sprite.setScale(currentScale.x, std::abs(currentScale.y));
            sprite.setPosition(revivalBasePos);
        }
    }

    // Update the player's HP bar (position above the sprite and width based on HP or revival progress)
    void updateHpBar() {
        float hpRatio;
        if (isReviving) {
            // During revival, show revival progress on HP bar
            float elapsed = revivalClock.getElapsedTime().asSeconds();
            hpRatio = std::min(elapsed / revivalTime, 1.0f);
        } else {
            hpRatio = static_cast<float>(hp) / static_cast<float>(maxHp);
        }
        float barWidth = 64.0f;
        float barHeight = 8.0f;
        hpBar.setSize(sf::Vector2f(barWidth * hpRatio, barHeight));
        // Position the bar slightly above the player's sprite
        sf::Vector2f pos = sprite.getPosition();
        float offsetY = FRAME_HEIGHT * 0.2f;
        hpBarBackground.setPosition(pos.x - barWidth / 2.0f, pos.y - offsetY);
        hpBar.setPosition(pos.x - barWidth / 2.0f, pos.y - offsetY);
    }
};

// Static member definitions for Player stat limits and upgrade values
float Player::MOVE_SPEED_MAX = 0.08f;
float Player::CLEAN_SPEED_MIN = 0.2f;
int   Player::MAX_HP_MAX = 300;
int   Player::ATTACK_POWER_MAX = 100;
float Player::ATTACK_SPEED_MIN = 0.3f;
float Player::REVIVAL_TIME_MIN = 0.1f;
float Player::MOVE_SPEED_MIN = 0.001f;
float Player::CLEAN_SPEED_MAX = 4.0f;
int   Player::MAX_HP_MIN = 10;
int   Player::ATTACK_POWER_MIN = 1;
float Player::ATTACK_SPEED_MAX = 5.0f;
float Player::REVIVAL_TIME_MAX = 10.0f;
float Player::MOVE_SPEED_UP = 0.01f;
float Player::CLEAN_SPEED_UP = -0.5f;
int   Player::MAX_HP_UP = 50;
int   Player::ATTACK_POWER_UP = 15;
float Player::ATTACK_SPEED_UP = -0.5f;
float Player::REVIVAL_TIME_DOWN = -1.0f;
float Player::MOVE_SPEED_DOWN = -0.003f;
float Player::CLEAN_SPEED_DOWN = 0.2f;
int   Player::MAX_HP_DOWN = -20;
int   Player::ATTACK_POWER_DOWN = -2;
float Player::ATTACK_SPEED_DOWN = 0.3f;
float Player::REVIVAL_TIME_UP = 0.3f;

// Enemy class representing an enemy with stats and sprite
class Enemy {
public:
    sf::Sprite sprite;
    float moveSpeed;
    int hp;
    int maxHp;
    int attackPower;
    float attackSpeed;

    // Sprite sheet frame constants for enemy
    static constexpr int FRAME_WIDTH = 260;
    static constexpr int FRAME_HEIGHT = 250;
    static constexpr int FRAME_COUNT = 4;

    // Constructor initializes the enemy sprite and stats
    Enemy(sf::Texture& texture, const sf::Vector2f& position) {
        sprite.setTexture(texture);
        // Set initial animation frame for enemy
        sprite.setTextureRect(sf::IntRect(30, 380, FRAME_WIDTH, FRAME_HEIGHT));
        sprite.setScale(0.3f, 0.3f);
        // Center origin of the enemy sprite
        sf::FloatRect enemyBounds = sprite.getLocalBounds();
        sprite.setOrigin(enemyBounds.width / 2.0f, enemyBounds.height / 2.0f);
        sprite.setPosition(position);
        // Default stats for enemy
        moveSpeed = 0.01f;
        hp = 50;
        maxHp = 50;
        attackPower = 10;
        attackSpeed = 1.0f;
    }
};

// Game class manages the overall game state and logic
class Game {
public:
    Game();
    void run();

private:
    // Window and current state
    sf::RenderWindow window;
    GameState currentState;

    // Game objects and resources
    Player player;
    std::vector<Enemy> enemies;
    std::vector<sf::Sprite> trashSprites;
    sf::Texture trashTexture;
    sf::Texture enemyTexture;

    // Font and UI elements (texts and shapes)
    sf::Font font;
    sf::RectangleShape startButton;
    sf::Text startText;
    sf::Text timerText;
    sf::Text clearText;
    sf::Text gameOverText;
    sf::RectangleShape restartButton;
    sf::Text restartText;
    sf::RectangleShape expBarBackground;
    sf::RectangleShape expBar;
    sf::RectangleShape pollutionBarBackground;
    sf::RectangleShape pollutionBarFill;

    // Upgrade menu UI
    bool showUpgradeScreen;
    sf::RectangleShape upgradeOverlay;
    std::vector<sf::RectangleShape> upgradeItems;
    std::array<sf::Texture, 6> upgradeTextures;
    std::array<std::string, 6> upgradePaths;
    std::array<std::string, 6> upgradeDescriptions;
    sf::Text statsText;
    std::map<std::string, sf::Color> statChangeColors;
    std::vector<std::pair<std::string, float>> upcomingChanges;

    // Game timing and mechanics
    sf::Clock gameClock;
    sf::Time activeTime;
    const sf::Time timeLimit;
    sf::Clock spawnClock;
    sf::Time spawnTimeAcc;
    float trashSpawnInterval;
    size_t maxTrashCount;
    float pollution;
    const float pollutionIncrement;
    const float pollutionMax;
    sf::Clock enemySpawnClock;
    float enemySpawnInterval;
    const size_t enemyMaxCount;
    sf::Clock enemyAnimationClock;
    int currentEnemyAnimFrame;
    const float enemyAnimationSpeed;
    bool enemyCollisionPending;
    int collidedEnemyIndex;
    sf::Clock playerAttackClock;
    sf::Clock enemyAttackClock;
    bool collisionPending;
    int collidedTrashIndex;
    sf::Clock collisionClock;
    int upgradeCount;
    float experience;
    float getExperience;        // experience gained per trash cleaned
    const float experiencePerEnemy;  // experience gained per enemy defeated
    const float maxExperience;  // experience threshold for level-up

    // Utility: compare floats for near-equality
    static bool isNearlyEqual(float a, float b, float precision = 0.001f) {
        return std::abs(a - b) <= precision;
    }

    // Core game loop sub-functions
    void handleEvents();
    void update();
    void updateGamePlay();
    void render();

    // Helper functions for specific actions
    void handleUpgradeSelection(int index);
    void spawnTrash();
    void spawnEnemy();
    void resetGame();

    // Drawing sub-functions for different screens
    void drawTitleScreen();
    void drawGameScreen();
    void drawClearScreen();
    void drawGameOverScreen();
    void drawUpgradeScreen();
};

Game::Game()
: window(sf::VideoMode(800, 600), "Screen Transition Example"),
  currentState(GameState::Title),
  timeLimit(sf::seconds(300)),
  spawnTimeAcc(sf::Time::Zero),
  trashSpawnInterval(3.0f),
  maxTrashCount(100),
  pollution(0.0f),
  pollutionIncrement(5.0f),
  pollutionMax(100.0f),
  enemySpawnInterval(5.0f),
  enemyMaxCount(3),
  currentEnemyAnimFrame(0),
  enemyAnimationSpeed(0.2f),
  enemyCollisionPending(false),
  collidedEnemyIndex(-1),
  collisionPending(false),
  collidedTrashIndex(-1),
  upgradeCount(0),
  experience(0.0f),
  getExperience(50.0f),
  experiencePerEnemy(30.0f),
  maxExperience(100.0f)
{
    // Seed the random number generator
    std::srand(static_cast<unsigned>(std::time(nullptr)));

    // Load textures
    if (!trashTexture.loadFromFile("../image/trush.png")) {
        std::cerr << "Failed to load trush.png" << std::endl;
        std::exit(-1);
    }
    if (!enemyTexture.loadFromFile("../image/enemy1.png")) {
        std::cerr << "Failed to load enemy1.png" << std::endl;
        std::exit(-1);
    }
    // Load font for text
    if (!font.loadFromFile("/System/Library/Fonts/Supplemental/Arial.ttf")) {
        std::cerr << "Failed to load font." << std::endl;
        std::exit(-1);
    }

    // Title screen "Game Start" button setup
    startButton = sf::RectangleShape(sf::Vector2f(200, 100));
    startButton.setPosition(300, 250);
    startButton.setFillColor(sf::Color::Blue);
    startText.setFont(font);
    startText.setString("Game Start");
    startText.setCharacterSize(30);
    startText.setFillColor(sf::Color::White);
    sf::FloatRect btnBounds = startText.getLocalBounds();
    startText.setOrigin(btnBounds.left + btnBounds.width / 2.0f,
                        btnBounds.top + btnBounds.height / 2.0f);
    startText.setPosition(startButton.getPosition().x + startButton.getSize().x / 2.0f,
                          startButton.getPosition().y + startButton.getSize().y / 2.0f);

    // Timer text (red countdown at top)
    timerText.setFont(font);
    timerText.setCharacterSize(40);
    timerText.setFillColor(sf::Color::Red);
    timerText.setPosition(250, 20);

    // "Clear!" text
    clearText.setFont(font);
    clearText.setString("Clear!");
    clearText.setCharacterSize(60);
    clearText.setFillColor(sf::Color::Green);
    sf::FloatRect clearBounds = clearText.getLocalBounds();
    clearText.setOrigin(clearBounds.width / 2.0f, clearBounds.height / 2.0f);
    clearText.setPosition(400, 300);

    // "GameOver..." text
    gameOverText.setFont(font);
    gameOverText.setString("GameOver...");
    gameOverText.setCharacterSize(60);
    gameOverText.setFillColor(sf::Color::Red);
    sf::FloatRect overBounds = gameOverText.getLocalBounds();
    gameOverText.setOrigin(overBounds.width / 2.0f, overBounds.height / 2.0f);
    gameOverText.setPosition(400, 300);

    // Restart button and label
    restartButton = sf::RectangleShape(sf::Vector2f(200, 60));
    restartButton.setFillColor(sf::Color::Green);
    restartButton.setPosition(300, 400);
    restartText.setFont(font);
    restartText.setString("Restart");
    restartText.setCharacterSize(30);
    restartText.setFillColor(sf::Color::White);
    sf::FloatRect restartTextBounds = restartText.getLocalBounds();
    restartText.setOrigin(restartTextBounds.left + restartTextBounds.width / 2.0f,
                          restartTextBounds.top + restartTextBounds.height / 2.0f);
    restartText.setPosition(restartButton.getPosition().x + restartButton.getSize().x / 2.0f,
                            restartButton.getPosition().y + restartButton.getSize().y / 2.0f);

    // Experience bar (green bar at top-right)
    expBarBackground = sf::RectangleShape(sf::Vector2f(300, 20));
    expBarBackground.setPosition(500, 20);
    expBarBackground.setFillColor(sf::Color(50, 50, 50));
    expBar = sf::RectangleShape(sf::Vector2f(0, 20));
    expBar.setPosition(500, 20);
    expBar.setFillColor(sf::Color(0, 255, 0));

    // Pollution meter (blue vertical bar on left)
    pollutionBarBackground = sf::RectangleShape(sf::Vector2f(30, 300));
    pollutionBarBackground.setPosition(20, 20);
    pollutionBarBackground.setFillColor(sf::Color(100, 100, 100));
    pollutionBarFill = sf::RectangleShape(sf::Vector2f(30, 0));
    pollutionBarFill.setFillColor(sf::Color(0, 0, 255));
    pollutionBarFill.setPosition(20, 20 + 300);

    // Upgrade screen overlay setup
    showUpgradeScreen = false;
    upgradeOverlay = sf::RectangleShape(sf::Vector2f(800, 600));
    upgradeOverlay.setFillColor(sf::Color(0, 0, 0, 150));
    upgradeItems.clear();
    // Calculate positions for 6 upgrade icons
    int numUpgradeItems = 6;
    sf::Vector2f itemSize(100, 100);
    float spacing = 20.0f;
    float totalWidth = numUpgradeItems * itemSize.x + (numUpgradeItems - 1) * spacing;
    float startX = (800 - totalWidth) / 2.0f;
    float posY = 250.0f;
    for (int i = 0; i < numUpgradeItems; ++i) {
        sf::RectangleShape item(itemSize);
        float posX = startX + i * (itemSize.x + spacing);
        item.setPosition(posX, posY);
        item.setFillColor(sf::Color::White);
        upgradeItems.push_back(item);
    }
    // Upgrade icons file paths and descriptions
    upgradePaths = {"../image/moveSpeed.jpg",
                    "../image/cleanPower.jpg",
                    "../image/hpPower.jpg",
                    "../image/attackPower.jpg",
                    "../image/attackSpeed.jpg",
                    "../image/revivalTime.jpg"};
    upgradeDescriptions = {"Boost your speed!",
                           "Clean trash faster!",
                           "Max HP up!",
                           "Hit harder!",
                           "Attack faster!",
                           "Revive quicker!"};
    // Load upgrade icon textures
    for (int i = 0; i < numUpgradeItems; ++i) {
        if (!upgradeTextures[i].loadFromFile(upgradePaths[i])) {
            std::cerr << "Failed to load " << upgradePaths[i] << std::endl;
        }
        upgradeItems[i].setTexture(&upgradeTextures[i]);
    }
    // Stat change colors (green = positive effect, red = negative effect)
    statChangeColors = {
        {"moveSpeedUP", sf::Color::Green},   {"cleanSpeedUP", sf::Color::Green},
        {"maxHpUP", sf::Color::Green},       {"attackPowerUP", sf::Color::Green},
        {"attackSpeedUP", sf::Color::Green}, {"revivalTimeDOWN", sf::Color::Green},
        {"moveSpeedDOWN", sf::Color::Red},   {"cleanSpeedDOWN", sf::Color::Red},
        {"maxHpDOWN", sf::Color::Red},       {"attackPowerDOWN", sf::Color::Red},
        {"attackSpeedDOWN", sf::Color::Red}, {"revivalTimeUP", sf::Color::Red}
    };
    statsText.setFont(font);
    statsText.setCharacterSize(20);
    statsText.setFillColor(sf::Color::White);
    statsText.setPosition(500, 30);
    upcomingChanges.clear();

    // Spawn one initial trash on the field
    spawnTrash();
}

// Run the main game loop
void Game::run() {
    while (window.isOpen()) {
        handleEvents();
        update();
        render();
    }
}

// Handle input events for all game states
void Game::handleEvents() {
    sf::Event event;
    while (window.pollEvent(event)) {
        // Always handle window close
        if (event.type == sf::Event::Closed) {
            window.close();
            return;
        }
        // If the upgrade menu is open, handle upgrade item click and skip other events
        if (showUpgradeScreen) {
            if (event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
                sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                for (int i = 0; i < upgradeItems.size(); ++i) {
                    if (upgradeItems[i].getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                        // Apply selected upgrade
                        handleUpgradeSelection(i);
                        showUpgradeScreen = false;
                        // Restart game clock to avoid time jump after pause
                        gameClock.restart();
                        break;
                    }
                }
            }
            // Skip processing other events while in upgrade screen
            continue;
        }

        // Title screen: start game on "Game Start" button click
        if (currentState == GameState::Title) {
            if (event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
                sf::Vector2i mousePos = sf::Mouse::getPosition(window);
                if (startButton.getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                    std::cout << "Button Clicked! Transitioning to Game Screen." << std::endl;
                    currentState = GameState::Game;
                    // Load player sprite and reset game clocks when starting the game
                    if (!player.loaded) {
                        if (!player.loadSprite("../image/hermitcrab_spritesheet.png")) {
                            std::exit(-1);
                        }
                    }
                    activeTime = sf::Time::Zero;
                    gameClock.restart();
                    spawnClock.restart();
                    enemySpawnClock.restart();
                }
            }
        }

        // End screens: restart game on "Restart" button click
        if ((currentState == GameState::GameOver || currentState == GameState::Clear) &&
            event.type == sf::Event::MouseButtonPressed && event.mouseButton.button == sf::Mouse::Left) {
            sf::Vector2i mousePos = sf::Mouse::getPosition(window);
            if (restartButton.getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
                std::cout << "Restarting game..." << std::endl;
                resetGame();
            }
        }
    }
}

// Update game logic based on the current state
void Game::update() {
    if (currentState == GameState::Game) {
        updateGamePlay();
    }
    // No updates needed for Title, Clear, or GameOver screens (they are static)
}

// Update the gameplay state (called each frame during Game state)
void Game::updateGamePlay() {
    if (!showUpgradeScreen) {
        // Advance game time if not paused by upgrade screen
        sf::Time deltaTime = gameClock.restart();
        activeTime += deltaTime;

        // Update player animation if applicable (not colliding or reviving)
        if (!collisionPending && !enemyCollisionPending && !trashSprites.empty() && !player.isReviving) {
            player.updateAnimation();
        }
        // Update enemy animations (cycle through sprite frames)
        if (enemyAnimationClock.getElapsedTime().asSeconds() >= enemyAnimationSpeed) {
            currentEnemyAnimFrame = (currentEnemyAnimFrame + 1) % Enemy::FRAME_COUNT;
            for (auto& enemy : enemies) {
                enemy.sprite.setTextureRect(sf::IntRect(currentEnemyAnimFrame * Enemy::FRAME_WIDTH, 400, 
                                                       Enemy::FRAME_WIDTH, Enemy::FRAME_HEIGHT));
            }
            enemyAnimationClock.restart();
        }
        // Spawn new trash periodically
        sf::Time spawnDelta = spawnClock.restart();
        spawnTimeAcc += spawnDelta;
        if (spawnTimeAcc.asSeconds() >= trashSpawnInterval) {
            if (trashSprites.size() < maxTrashCount) {
                spawnTrash();
            }
            spawnTimeAcc -= sf::seconds(trashSpawnInterval);
        }
        // Update countdown timer and check for level clear
        sf::Time remaining = timeLimit - activeTime;
        if (remaining <= sf::Time::Zero) {
            currentState = GameState::Clear;
        } else {
            int secondsLeft = static_cast<int>(remaining.asSeconds());
            timerText.setString("Time: " + std::to_string(secondsLeft));
        }
        // Spawn new enemy periodically (if player is not reviving)
        if (!player.isReviving && enemySpawnClock.getElapsedTime().asSeconds() >= enemySpawnInterval) {
            if (enemies.size() < enemyMaxCount) {
                spawnEnemy();
            }
            enemySpawnClock.restart();
        }
        // Move enemies toward player (if not in combat or player reviving)
        if (!enemyCollisionPending && !player.isReviving) {
            for (auto& enemy : enemies) {
                sf::Vector2f enemyPos = enemy.sprite.getPosition();
                sf::Vector2f playerPos = player.sprite.getPosition();
                sf::Vector2f dir = playerPos - enemyPos;
                float dist = std::sqrt(dir.x * dir.x + dir.y * dir.y);
                // Face the player
                if (dir.x < 0) {
                    enemy.sprite.setScale(-0.3f, 0.3f);
                } else if (dir.x > 0) {
                    enemy.sprite.setScale(0.3f, 0.3f);
                }
                // Move enemy if not already very close
                if (dist > 0.1f) {
                    dir /= dist;
                    enemy.sprite.move(dir * enemy.moveSpeed);
                }
            }
        }
        // Check for collision between player and any enemy
        if (!enemyCollisionPending) {
            for (size_t i = 0; i < enemies.size(); ++i) {
                if (player.sprite.getGlobalBounds().intersects(enemies[i].sprite.getGlobalBounds())) {
                    enemyCollisionPending = true;
                    collidedEnemyIndex = static_cast<int>(i);
                    // Start combat timers for both player and enemy
                    playerAttackClock.restart();
                    enemyAttackClock.restart();
                    break;
                }
            }
        }
        // Handle player-enemy combat if a collision is in progress
        if (enemyCollisionPending && collidedEnemyIndex >= 0) {
            // Player attacks enemy at specified attack speed interval
            if (playerAttackClock.getElapsedTime().asSeconds() >= player.attackSpeed) {
                std::cout << "Player attacks enemy." << std::endl;
                std::cout << playerAttackClock.getElapsedTime().asSeconds() << std::endl;
                enemies[collidedEnemyIndex].hp -= player.attackPower;
                playerAttackClock.restart();
            }
            // Enemy attacks player at its attack speed interval
            if (enemyAttackClock.getElapsedTime().asSeconds() >= enemies[collidedEnemyIndex].attackSpeed) {
                std::cout << "Enemy attacks player." << std::endl;
                player.hp -= enemies[collidedEnemyIndex].attackPower;
                enemyAttackClock.restart();
            }
            // Check if enemy was defeated
            if (enemies[collidedEnemyIndex].hp <= 0) {
                std::cout << "エネミーやられた" << std::endl;
                experience += experiencePerEnemy;
                enemies.erase(enemies.begin() + collidedEnemyIndex);
                collidedEnemyIndex = -1;
                enemyCollisionPending = false;
                // If enough experience for level-up, show upgrade screen
                if (experience >= maxExperience) {
                    std::cout << "Experience reached maximum. Showing upgrade screen." << std::endl;
                    experience -= maxExperience;
                    showUpgradeScreen = true;
                }
            }
            // Check if player was defeated
            if (player.hp <= 0) {
                std::cout << "やられた" << std::endl;
                // Remove any enemies currently colliding with the player
                for (size_t i = 0; i < enemies.size();) {
                    if (player.sprite.getGlobalBounds().intersects(enemies[i].sprite.getGlobalBounds())) {
                        enemies.erase(enemies.begin() + i);
                    } else {
                        ++i;
                    }
                }
                // Initiate player revival process
                player.isReviving = true;
                player.revivalBasePos = player.sprite.getPosition();
                player.revivalClock.restart();
                player.reviveWiggleToggle = false;
                // End combat state
                enemyCollisionPending = false;
            }
        }
        // Move player toward nearest trash (if not currently colliding or reviving)
        if (!collisionPending && !enemyCollisionPending && !trashSprites.empty() && !player.isReviving) {
            sf::Vector2f playerPos = player.sprite.getPosition();
            // Find the closest trash sprite
            float minDistSq = std::numeric_limits<float>::max();
            size_t closestIndex = 0;
            for (size_t i = 0; i < trashSprites.size(); ++i) {
                sf::Vector2f trashPos = trashSprites[i].getPosition();
                float dx = trashPos.x - playerPos.x;
                float dy = trashPos.y - playerPos.y;
                float distSq = dx * dx + dy * dy;
                if (distSq < minDistSq) {
                    minDistSq = distSq;
                    closestIndex = i;
                }
            }
            // Move player toward that trash
            sf::Vector2f targetPos = trashSprites[closestIndex].getPosition();
            sf::Vector2f direction = targetPos - playerPos;
            float length = std::sqrt(direction.x * direction.x + direction.y * direction.y);
            if (length > 0.1f) {
                direction /= length;
                player.sprite.move(direction * player.moveSpeed);
            }
            // Flip player sprite based on movement direction
            if (direction.x < 0) {
                player.sprite.setScale(-0.3f, 0.3f);
            } else if (direction.x > 0) {
                player.sprite.setScale(0.3f, 0.3f);
            }
        }
        // Check for collision between player and trash
        if (!collisionPending && !trashSprites.empty()) {
            // We will check collision with the nearest trash (for efficiency)
            sf::Vector2f playerPos = player.sprite.getPosition();
            size_t nearestIndex = 0;
            float minDistance = std::numeric_limits<float>::max();
            for (size_t i = 0; i < trashSprites.size(); ++i) {
                sf::Vector2f trashPos = trashSprites[i].getPosition();
                float dx = playerPos.x - trashPos.x;
                float dy = playerPos.y - trashPos.y;
                float distance = std::sqrt(dx * dx + dy * dy);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
            // If the player sprite intersects this nearest trash sprite, initiate cleaning
            if (player.sprite.getGlobalBounds().intersects(trashSprites[nearestIndex].getGlobalBounds())) {
                collisionPending = true;
                collidedTrashIndex = static_cast<int>(nearestIndex);
                collisionClock.restart();
            }
        }
        // Handle ongoing trash cleaning
        if (collisionPending) {
            if (collisionClock.getElapsedTime().asSeconds() >= player.cleanSpeed) {
                // Trash cleaned up
                experience += getExperience;
                trashSprites.erase(trashSprites.begin() + collidedTrashIndex);
                // Decrease pollution level
                pollution = std::max(pollution - pollutionIncrement, 0.0f);
                collisionPending = false;
                // If enough experience for level-up, trigger upgrade screen
                if (experience >= maxExperience) {
                    std::cout << "Experience reached maximum. Showing upgrade screen." << std::endl;
                    experience -= maxExperience;
                    ++upgradeCount;
                    // Adjust future trash experience gain (to slow down leveling each time)
                    getExperience = std::max(10.0f, 50.0f / (1.0f + upgradeCount * 0.5f));
                    showUpgradeScreen = true;
                }
            }
        }
        // Update experience bar length according to current experience
        float expWidth = 300 * (experience / maxExperience);
        expBar.setSize(sf::Vector2f(expWidth, 20));

        // Update player revival state (if in reviving mode)
        if (player.isReviving) {
            player.updateRevival();
        } else {
            // Ensure player sprite is upright when not reviving
            sf::Vector2f currentScale = player.sprite.getScale();
            player.sprite.setScale(currentScale.x, std::abs(currentScale.y));
        }
        // Update the player's HP bar position/size each frame
        player.updateHpBar();
    } else {
        // If upgrade screen is open, pause game and reset clocks to prevent time accumulation
        gameClock.restart();
        spawnClock.restart();
        enemySpawnClock.restart();
    }

    // Update pollution bar fill to reflect current pollution level
    float fillHeight = (pollution / pollutionMax) * 300.0f;
    pollutionBarFill.setSize(sf::Vector2f(30, fillHeight));
    pollutionBarFill.setPosition(20, 20 + 300 - fillHeight);
}

// Render the current frame based on game state
void Game::render() {
    window.clear(sf::Color::Black);
    if (currentState == GameState::Title) {
        drawTitleScreen();
    } else if (currentState == GameState::Game) {
        drawGameScreen();
    } else if (currentState == GameState::Clear) {
        drawClearScreen();
    } else if (currentState == GameState::GameOver) {
        drawGameOverScreen();
    }
    window.display();
}

// Apply the effects of an upgrade selection (when an upgrade icon is clicked)
void Game::handleUpgradeSelection(int index) {
    std::string fileName = upgradePaths[index];
    upcomingChanges.clear();
    if (fileName.find("moveSpeed.jpg") != std::string::npos) {
        player.moveSpeed = std::min(player.moveSpeed + Player::MOVE_SPEED_UP, Player::MOVE_SPEED_MAX);
        player.revivalTime = std::min(player.revivalTime + Player::REVIVAL_TIME_UP, Player::REVIVAL_TIME_MAX);
        upcomingChanges.emplace_back("moveSpeed", Player::MOVE_SPEED_UP);
        upcomingChanges.emplace_back("revivalTime", Player::REVIVAL_TIME_UP);
    }
    if (fileName.find("cleanPower.jpg") != std::string::npos) {
        player.cleanSpeed = std::max(player.cleanSpeed + Player::CLEAN_SPEED_UP, Player::CLEAN_SPEED_MIN);
        player.attackPower = std::max(player.attackPower + Player::ATTACK_POWER_DOWN, Player::ATTACK_POWER_MIN);
        upcomingChanges.emplace_back("cleanSpeed", Player::CLEAN_SPEED_UP);
        upcomingChanges.emplace_back("attackPower", Player::ATTACK_POWER_DOWN);
    }
    if (fileName.find("hpPower.jpg") != std::string::npos) {
        player.maxHp = std::min(player.maxHp + Player::MAX_HP_UP, Player::MAX_HP_MAX);
        player.hp += Player::MAX_HP_UP;  // increase current HP as well
        player.moveSpeed = std::max(player.moveSpeed + Player::MOVE_SPEED_DOWN, Player::MOVE_SPEED_MIN);
        upcomingChanges.emplace_back("maxHp", Player::MAX_HP_UP);
        upcomingChanges.emplace_back("moveSpeedDown", Player::MOVE_SPEED_DOWN);
    }
    if (fileName.find("attackPower.jpg") != std::string::npos) {
        player.attackPower = std::min(player.attackPower + Player::ATTACK_POWER_UP, Player::ATTACK_POWER_MAX);
        player.attackSpeed = std::min(player.attackSpeed + Player::ATTACK_SPEED_DOWN, Player::ATTACK_SPEED_MAX);
        upcomingChanges.emplace_back("attackPower", Player::ATTACK_POWER_UP);
        upcomingChanges.emplace_back("attackSpeedUp", Player::ATTACK_SPEED_DOWN);
    }
    if (fileName.find("attackSpeed.jpg") != std::string::npos) {
        player.attackSpeed = std::max(player.attackSpeed + Player::ATTACK_SPEED_UP, Player::ATTACK_SPEED_MIN);
        player.cleanSpeed = std::min(player.cleanSpeed + Player::CLEAN_SPEED_DOWN, Player::CLEAN_SPEED_MAX);
        upcomingChanges.emplace_back("attackSpeed", Player::ATTACK_SPEED_UP);
        upcomingChanges.emplace_back("cleanSpeedUp", Player::CLEAN_SPEED_DOWN);
    }
    if (fileName.find("revivalTime.jpg") != std::string::npos) {
        player.revivalTime = std::max(player.revivalTime + Player::REVIVAL_TIME_DOWN, Player::REVIVAL_TIME_MIN);
        player.maxHp = std::max(player.maxHp + Player::MAX_HP_DOWN, Player::MAX_HP_MIN);
        if (player.hp > player.maxHp) {
            player.hp = player.maxHp;
        }
        upcomingChanges.emplace_back("revivalTime", Player::REVIVAL_TIME_DOWN);
        upcomingChanges.emplace_back("maxHpDown", Player::MAX_HP_DOWN);
    }
}

// Spawn a new trash (garbage) sprite at a random position on the field
void Game::spawnTrash() {
    if (trashSprites.size() >= maxTrashCount) return;
    sf::Sprite newTrash;
    newTrash.setTexture(trashTexture);
    // Set origin to center (using texture's size before scaling)
    sf::FloatRect bounds = newTrash.getLocalBounds();
    newTrash.setOrigin(bounds.width / 2.0f, bounds.height / 2.0f);
    newTrash.setScale(0.07f, 0.07f);
    float randX = static_cast<float>(std::rand() % 700 + 50);
    float randY = static_cast<float>(std::rand() % 500 + 50);
    newTrash.setPosition(randX, randY);
    trashSprites.push_back(newTrash);
    // Increase pollution and check for Game Over condition
    pollution = std::min(pollution + pollutionIncrement, pollutionMax);
    if (pollution >= pollutionMax) {
        std::cout << "Pollution reached maximum. Game Over." << std::endl;
        currentState = GameState::GameOver;
    }
}

// Spawn a new enemy at a random edge of the window
void Game::spawnEnemy() {
    int edge = std::rand() % 4;
    float x = 0.f, y = 0.f;
    if (edge == 0) {          // top edge
        x = static_cast<float>(std::rand() % 800);
        y = 0.f;
    } else if (edge == 1) {   // bottom edge
        x = static_cast<float>(std::rand() % 800);
        y = 600.f;
    } else if (edge == 2) {   // left edge
        x = 0.f;
        y = static_cast<float>(std::rand() % 600);
    } else {                  // right edge
        x = 800.f;
        y = static_cast<float>(std::rand() % 600);
    }
    Enemy newEnemy(enemyTexture, sf::Vector2f(x, y));
    enemies.push_back(newEnemy);
}

// Reset the game state to initial conditions (for restarting the game)
void Game::resetGame() {
    currentState = GameState::Title;
    activeTime = sf::Time::Zero;
    gameClock.restart();
    spawnClock.restart();
    enemySpawnClock.restart();
    experience = 0.0f;
    upgradeCount = 0;
    getExperience = 50.0f;
    pollution = 0.0f;
    // Reset player stats to defaults
    player.moveSpeed = 0.01f;
    player.cleanSpeed = 2.0f;
    player.hp = 100;
    player.maxHp = 100;
    player.attackPower = 10;
    player.attackSpeed = 2.0f;
    player.revivalTime = 5.0f;
    player.isReviving = false;
    // Mark player sprite as not loaded, so it will reload on next game start
    player.loaded = false;
    // Clear existing enemies and trash, then spawn one initial trash
    enemies.clear();
    trashSprites.clear();
    collidedEnemyIndex = -1;
    enemyCollisionPending = false;
    collisionPending = false;
    spawnTrash();
}

// Drawing the Title screen (Start button)
void Game::drawTitleScreen() {
    window.draw(startButton);
    window.draw(startText);
}

// Drawing the active game screen (gameplay and HUD)
void Game::drawGameScreen() {
    // Draw all trash sprites
    for (const auto& trash : trashSprites) {
        window.draw(trash);
    }
    // Draw all enemy sprites
    for (const auto& enemy : enemies) {
        window.draw(enemy.sprite);
    }
    // Draw the player sprite
    window.draw(player.sprite);
    // Draw UI elements: experience bar, pollution bar, timer
    window.draw(expBarBackground);
    window.draw(expBar);
    window.draw(pollutionBarBackground);
    window.draw(pollutionBarFill);
    window.draw(timerText);
    // Draw player HP bar
    window.draw(player.hpBarBackground);
    window.draw(player.hpBar);
    // Draw each enemy's HP bar above it
    float barWidth = 64.0f;
    for (const auto& enemy : enemies) {
        float enemyHpRatio = static_cast<float>(enemy.hp) / static_cast<float>(enemy.maxHp);
        sf::RectangleShape enemyHpBarBack(sf::Vector2f(barWidth, 6.0f));
        sf::RectangleShape enemyHpBarFill(sf::Vector2f(barWidth * enemyHpRatio, 6.0f));
        enemyHpBarBack.setFillColor(sf::Color(100, 100, 100));
        enemyHpBarFill.setFillColor(sf::Color::Green);
        sf::Vector2f ePos = enemy.sprite.getPosition();
        float enemyOffsetY = Enemy::FRAME_HEIGHT * 0.2f;
        enemyHpBarBack.setPosition(ePos.x - barWidth / 2.0f, ePos.y - enemyOffsetY);
        enemyHpBarFill.setPosition(ePos.x - barWidth / 2.0f, ePos.y - enemyOffsetY);
        window.draw(enemyHpBarBack);
        window.draw(enemyHpBarFill);
    }
    // If the upgrade screen is active, overlay it on top
    if (showUpgradeScreen) {
        drawUpgradeScreen();
    }
}

// Drawing the "Clear" screen (game win)
void Game::drawClearScreen() {
    window.draw(clearText);
    window.draw(restartButton);
    window.draw(restartText);
}

// Drawing the "GameOver" screen
void Game::drawGameOverScreen() {
    window.draw(gameOverText);
    window.draw(restartButton);
    window.draw(restartText);
}

// Drawing the upgrade selection overlay and stat panel
void Game::drawUpgradeScreen() {
    window.draw(upgradeOverlay);
    bool hovered = false;
    sf::Vector2i mousePos = sf::Mouse::getPosition(window);
    // Draw each upgrade icon and check for hover
    for (size_t i = 0; i < upgradeItems.size(); ++i) {
        window.draw(upgradeItems[i]);
        if (upgradeItems[i].getGlobalBounds().contains(static_cast<float>(mousePos.x), static_cast<float>(mousePos.y))) {
            // Show description above the hovered icon
            sf::Text descriptionText(upgradeDescriptions[i], font, 18);
            descriptionText.setFillColor(sf::Color::White);
            sf::FloatRect iconBounds = upgradeItems[i].getGlobalBounds();
            sf::FloatRect textBounds = descriptionText.getLocalBounds();
            descriptionText.setOrigin(textBounds.left + textBounds.width / 2.0f,
                                      textBounds.top + textBounds.height);
            descriptionText.setPosition(iconBounds.left + iconBounds.width / 2.0f, iconBounds.top - 20.0f);
            window.draw(descriptionText);
            // Prepare stat change list for this upgrade option
            upcomingChanges.clear();
            if (i == 0) {
                upcomingChanges.emplace_back("moveSpeedUP", Player::MOVE_SPEED_UP);
                upcomingChanges.emplace_back("revivalTimeUP", Player::REVIVAL_TIME_UP);
            } else if (i == 1) {
                upcomingChanges.emplace_back("cleanSpeedUP", Player::CLEAN_SPEED_UP);
                upcomingChanges.emplace_back("attackPowerDOWN", Player::ATTACK_POWER_DOWN);
            } else if (i == 2) {
                upcomingChanges.emplace_back("maxHpUP", Player::MAX_HP_UP);
                upcomingChanges.emplace_back("moveSpeedDOWN", Player::MOVE_SPEED_DOWN);
            } else if (i == 3) {
                upcomingChanges.emplace_back("attackPowerUP", Player::ATTACK_POWER_UP);
                upcomingChanges.emplace_back("attackSpeedDOWN", Player::ATTACK_SPEED_DOWN);
            } else if (i == 4) {
                upcomingChanges.emplace_back("attackSpeedUP", Player::ATTACK_SPEED_UP);
                upcomingChanges.emplace_back("cleanSpeedDOWN", Player::CLEAN_SPEED_DOWN);
            } else if (i == 5) {
                upcomingChanges.emplace_back("revivalTimeDOWN", Player::REVIVAL_TIME_DOWN);
                upcomingChanges.emplace_back("maxHpDOWN", Player::MAX_HP_DOWN);
            }
            hovered = true;
        }
    }
    if (!hovered) {
        upcomingChanges.clear();
    }
    // Build stats text showing current player stats and limits
    std::ostringstream statsStream;
    statsStream << "[Current Stats]\n";
    statsStream << "Speed:        " << player.moveSpeed;
    if (isNearlyEqual(player.moveSpeed, Player::MOVE_SPEED_MAX))
        statsStream << " (max)";
    else if (isNearlyEqual(player.moveSpeed, Player::MOVE_SPEED_MIN))
        statsStream << " (min)";
    statsStream << "\n";
    statsStream << "CleanSpeed:   " << player.cleanSpeed << "s";
    if (isNearlyEqual(player.cleanSpeed, Player::CLEAN_SPEED_MAX))
        statsStream << " (max)";
    else if (isNearlyEqual(player.cleanSpeed, Player::CLEAN_SPEED_MIN))
        statsStream << " (min)";
    statsStream << "\n";
    statsStream << "MaxHP:        " << player.maxHp;
    if (player.maxHp >= Player::MAX_HP_MAX)
        statsStream << " (max)";
    else if (player.maxHp <= Player::MAX_HP_MIN)
        statsStream << " (min)";
    statsStream << "\n";
    statsStream << "AttackPower:  " << player.attackPower;
    if (isNearlyEqual(player.attackPower, Player::ATTACK_POWER_MAX))
        statsStream << " (max)";
    else if (isNearlyEqual(player.attackPower, Player::ATTACK_POWER_MIN))
        statsStream << " (min)";
    statsStream << "\n";
    statsStream << "AttackSpeed:  " << player.attackSpeed << "s";
    if (isNearlyEqual(player.attackSpeed, Player::ATTACK_SPEED_MAX))
        statsStream << " (max)";
    else if (isNearlyEqual(player.attackSpeed, Player::ATTACK_SPEED_MIN))
        statsStream << " (min)";
    statsStream << "\n";
    statsStream << "RevivalTime:  " << player.revivalTime << "s";
    if (isNearlyEqual(player.revivalTime, Player::REVIVAL_TIME_MAX))
        statsStream << " (max)";
    else if (isNearlyEqual(player.revivalTime, Player::REVIVAL_TIME_MIN))
        statsStream << " (min)";
    statsStream << "\n";
    statsText.setString(statsStream.str());
    window.draw(statsText);
    // Draw upcoming stat changes (if any) next to the stats
    float baseX = statsText.getPosition().x + 230.0f;
    float baseY = statsText.getPosition().y;
    float lineHeight = 23.0f;
    for (size_t j = 0; j < upcomingChanges.size(); ++j) {
        sf::Text changeText;
        changeText.setFont(font);
        changeText.setCharacterSize(20);
        // Format the change value with sign
        std::ostringstream val;
        val << std::showpos << upcomingChanges[j].second;
        changeText.setString(val.str());
        std::string key = upcomingChanges[j].first;
        // Set text color according to stat change (green or red)
        if (statChangeColors.count(key)) {
            changeText.setFillColor(statChangeColors[key]);
        } else {
            changeText.setFillColor(sf::Color::White);
        }
        // Position the change value next to the corresponding stat line
        if (key == "moveSpeedUP" || key == "moveSpeedDOWN") {
            changeText.setPosition(baseX, baseY + 1 * lineHeight);
        } else if (key == "cleanSpeedUP" || key == "cleanSpeedDOWN") {
            changeText.setPosition(baseX, baseY + 2 * lineHeight);
        } else if (key == "maxHpUP" || key == "maxHpDOWN") {
            changeText.setPosition(baseX, baseY + 3 * lineHeight);
        } else if (key == "attackPowerUP" || key == "attackPowerDOWN") {
            changeText.setPosition(baseX, baseY + 4 * lineHeight);
        } else if (key == "attackSpeedUP" || key == "attackSpeedDOWN") {
            changeText.setPosition(baseX, baseY + 5 * lineHeight);
        } else if (key == "revivalTimeUP" || key == "revivalTimeDOWN") {
            changeText.setPosition(baseX, baseY + 6 * lineHeight);
        }
        window.draw(changeText);
    }
}

int main() {
    Game game;
    game.run();
    return 0;
}
