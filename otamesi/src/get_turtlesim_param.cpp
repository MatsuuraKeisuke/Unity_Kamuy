#include <ros/ros.h>
#include <cstdlib>

int 
main(int argc, char **argv)
{
	ros::init(argc, argv, "get_background_color");
	ros::NodeHandle nh;

	int red_val, green_val, blue_val;

	if(nh.hasParam("/turtlesim/background_r"))
		nh.getParam("/turtlesim/background_r", red_val);
	if(nh.hasParam("/turtlesim/background_g"))
		nh.getParam("/turtlesim/background_g", green_val);
	if(nh.hasParam("/turtlesim/background_b"))
		nh.getParam("/turtlesim/background_b", blue_val);

	ROS_INFO_STREAM("background color: (" << red_val << ","
		<< green_val << ","
		<< blue_val << ")");

	return 0;
}
