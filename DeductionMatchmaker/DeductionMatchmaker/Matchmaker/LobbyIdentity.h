#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <string>
#include <iostream>

#pragma pack(push, 1)
class LobbyIdentity
{
public:
	std::string lobby;
	std::string address;
	int32_t port;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
