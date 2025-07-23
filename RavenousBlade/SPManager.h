#pragma once
#include <SFML/Graphics.hpp>

class GameContext;

class SPManager{
	public:
		enum class SPState {
			Idle,//必殺技を発動してない状態
			TimeStopStart,//必殺技押した直後。画面全体が暗転し、敵が停止
			BlinkStart,//目が閉じる
			BlinkEnd,//目が開くと同時に、テクスチャを差し替えて、真ん中の暗転が広がっていき、攻撃範囲が広がる
			Awake,//Blinkendの処理が完了
			Finishing,//敵が全滅した時
			Returning//円が小さくなって元の状態に戻る
		};
		SPState getCurrentState(){return currentState;};
		void setCurrentState(SPState newState){currentState = newState;};
		void checkSPRelease(GameContext&);
		void reset();
	private:
		SPState currentState = SPState::Idle;
		sf::Clock spClock; // 演出用タイマー
};