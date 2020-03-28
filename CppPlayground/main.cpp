#include <stdio.h>
#include <iostream>

class Location {
public:
	double_t Latitude;
	double_t Longitude;

	Location(double_t lat, double_t lon) {
		Latitude = lat;
		Longitude = lon;
	}
};

int main() {
	int* arr = new int[100];
	std::cout << arr << std::endl;
	arr[0] = 10;
	std::cout << arr << std::endl;
	delete[] arr;
	std::cout << arr << std::endl;

	Location* loc = new Location(2.8, 166.4);
	Location loc2(2.8, 123.3);

	std::shared_ptr<Location> arr2(new Location(2.4,5.6));

	system("pause"); // BAD
	return 0;
}