#pragma once

class GameState{
	public:
		virtual ~GameState();
		virtual void handleInput() = 0;
		virtual void gameUpdate() = 0;
		virtual void gameDraw() = 0;
};