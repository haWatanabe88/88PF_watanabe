#pragma once
#include <SFML/System.hpp>
#include <unordered_map>
#include <string>

class TimerUtils {
private:
    static std::unordered_map<std::string, sf::Clock> timers;

public:
    static void reset(const std::string& id) {
        timers[id].restart();
    }

    static bool hasElapsed(const std::string& id, float seconds) {
        return timers[id].getElapsedTime().asSeconds() >= seconds;
    }
};
