#include "SabotageTask.h"

SabotageTask::~SabotageTask()
{
	if (on_done)
		on_done();
}

void SabotageTask::fix()
{
	if (on_fixed)
		on_fixed();
}

void SabotageTask::expire()
{
	if (on_expired)
		on_expired();
}
