#pragma once

// WARNING : Auto-generated file, changes made will disappear when re-generated.

#include <string>
#include <vector>
#include <iostream>

#pragma pack(push, 1)
class SendSensorList
{
public:
	std::vector<std::string> names;

	void serialize(std::ostream& os) const;
	void deserialize(std::istream& is);
};
#pragma pack(pop)
