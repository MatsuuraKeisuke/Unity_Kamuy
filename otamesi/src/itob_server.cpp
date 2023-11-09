#include "ros/ros.h"
#include "otamesi/ItoB.h"

std::string itob(long int val)
{
    if(!val)
        return std::string("0");
    std::string str;
    while( val != 0 ) {
        if((val & 1) == 0)
            str.insert(str.begin(), '0');
        else
            str.insert(str.begin(), '1');
        val >>= 1;
    }
    return str;
}

bool convert(otamesi::ItoB::Request  &req,
             otamesi::ItoB::Response &res )
{
  res.binary = itob((long int)req.decimal);
  ROS_INFO_STREAM("request from client: (a decimal number) " << (long int)req.decimal);
  ROS_INFO_STREAM("  sending back response: (binay number) " << res.binary);
  return true;
}

int main(int argc, char **argv)
{
  ros::init(argc, argv, "itob_server");
  ros::NodeHandle n;

  ros::ServiceServer service = n.advertiseService("itob_service", convert);

  ros::spin();

  return 0;
}

