#include <ros/ros.h>
#include <std_srvs/Empty.h>
#include <cstdlib>

int 
main(int argc, char **argv)
{
	ros::init(argc, argv, "set_turtlesim");
	ros::NodeHandle nh;

	ros::service::waitForService("clear");

	srand(time(0));

	ros::Rate rate(10);
	while(ros::ok())
	{
		ros::param::set("/turtlesim/background_r", (int)rand()%255);
		ros::param::set("/turtlesim/background_g", (int)rand()%255);
		ros::param::set("/turtlesim/background_b", (int)rand()%255);

		ros::ServiceClient client = nh.serviceClient<std_srvs::Empty>("/clear");
		std_srvs::Empty srv;
		client.call(srv);
	
		rate.sleep();
	}
}

