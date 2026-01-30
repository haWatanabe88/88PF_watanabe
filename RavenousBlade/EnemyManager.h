#pragma once
#include <iostream>
#include "Enemy.h"

class GameContext;
class Enemy;

class EnemyManager{
	public:
		EnemyManager();
		enum class EnemyType{
			Normal,
			Fast,
			Slow,
			Boss
		};
		struct EnemySpawnSchedule{
			EnemyType type;
			Enemy::Direction direction;
			float spawnSeconds;
		};
		void initialScheduleList();
		std::vector<EnemySpawnSchedule>& getEnemySpawnScheduleList();
		Enemy* spawnEnemy(EnemyType, Enemy::Direction);
		/////
		std::vector<Enemy*>::iterator deleteEnemies(Enemy*, std::vector<Enemy*>&);
		bool loadAllTexture();
		void reset(GameContext&);
		sf::Texture& getNormalTextureFront(){return normal_texture_front;};
		sf::Texture& getNormalTextureLeft(){return normal_texture_left;};
		sf::Texture& getNormalTextureBack(){return normal_texture_back;};
		sf::Texture& getNormalTextureRight(){return normal_texture_right;};
		sf::Texture& getNormalTextureFrontAttack(){return normal_texture_front_attack;};
		sf::Texture& getNormalTextureLeftAttack(){return normal_texture_left_attack;};
		sf::Texture& getNormalTextureBackAttack(){return normal_texture_back_attack;};
		sf::Texture& getNormalTextureRightAttack(){return normal_texture_right_attack;};
		sf::Texture& getFastTextureFront(){return fast_texture_front;};
		sf::Texture& getFastTextureLeft(){return fast_texture_left;};
		sf::Texture& getFastTextureBack(){return fast_texture_back;};
		sf::Texture& getFastTextureRight(){return fast_texture_right;};
		sf::Texture& getFastTextureFrontAttack(){return fast_texture_front_attack;};
		sf::Texture& getFastTextureLeftAttack(){return fast_texture_left_attack;};
		sf::Texture& getFastTextureBackAttack(){return fast_texture_back_attack;};
		sf::Texture& getFastTextureRightAttack(){return fast_texture_right_attack;};
		sf::Texture& getSlowTextureFront(){return slow_texture_front;};
		sf::Texture& getSlowTextureLeft(){return slow_texture_left;};
		sf::Texture& getSlowTextureBack(){return slow_texture_back;};
		sf::Texture& getSlowTextureRight(){return slow_texture_right;};
		sf::Texture& getSlowTextureFrontAttack(){return slow_texture_front_attack;};
		sf::Texture& getSlowTextureLeftAttack(){return slow_texture_left_attack;};
		sf::Texture& getSlowTextureBackAttack(){return slow_texture_back_attack;};
		sf::Texture& getSlowTextureRightAttack(){return slow_texture_right_attack;};
/*
		sf::Texture& getBossTextureFront(){return boss_texture_front;};
		sf::Texture& getBossTextureLeft(){return boss_texture_left;};
		sf::Texture& getBossTextureBack(){return boss_texture_back;};
		sf::Texture& getBossTextureRight(){return boss_texture_right;};
		sf::Texture& getBossTextureFrontAttack(){return boss_texture_front_attack;};
		sf::Texture& getBossTextureLeftAttack(){return boss_texture_left_attack;};
		sf::Texture& getBossTextureBackAttack(){return boss_texture_back_attack;};
		sf::Texture& getBossTextureRightAttack(){return boss_texture_right_attack;};
*/
		void generateEnemies(GameContext&, float&);
		void updateEnemies(GameContext&);
	private:
		std::vector<EnemySpawnSchedule> enemySpawnScheduleList;
		sf::Texture normal_texture_front;
		sf::Texture normal_texture_left;
		sf::Texture normal_texture_back;
		sf::Texture normal_texture_right;
		sf::Texture normal_texture_front_attack;
		sf::Texture normal_texture_left_attack;
		sf::Texture normal_texture_back_attack;
		sf::Texture normal_texture_right_attack;

		sf::Texture fast_texture_front;
		sf::Texture fast_texture_left;
		sf::Texture fast_texture_back;
		sf::Texture fast_texture_right;
		sf::Texture fast_texture_front_attack;
		sf::Texture fast_texture_left_attack;
		sf::Texture fast_texture_back_attack;
		sf::Texture fast_texture_right_attack;
		sf::Texture slow_texture_front;
		sf::Texture slow_texture_left;
		sf::Texture slow_texture_back;
		sf::Texture slow_texture_right;
		sf::Texture slow_texture_front_attack;
		sf::Texture slow_texture_left_attack;
		sf::Texture slow_texture_back_attack;
		sf::Texture slow_texture_right_attack;
/*
		//Boss用
		sf::Texture boss_texture_front;
		sf::Texture boss_texture_left;
		sf::Texture boss_texture_back;
		sf::Texture boss_texture_right;
		sf::Texture boss_texture_front_attack;
		sf::Texture boss_texture_left_attack;
		sf::Texture boss_texture_back_attack;
		sf::Texture boss_texture_right_attack;
*/
};