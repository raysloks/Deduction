#include "SabotageTask.h"

SabotageTask::~SabotageTask()
{
	if (on_done)
		on_done();
}
