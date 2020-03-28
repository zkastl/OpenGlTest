#include <stdlib.h>
#include <iostream>
#include <vector>
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>

class Employee {
public:
	Employee(std::string first, std::string last) {
		first_name = first;
		last_name = last;
		employee_id = master_id;
		Employee::master_id++;
	}

	std::string GetName() const {
		return (first_name + " " + last_name);
	}

	void SetNewName(std::string new_first, std::string new_last) {
		first_name = new_first;
		last_name = new_last;
	}

	static uint32_t GetMasterId() {
		return master_id;
	}

	static void SetMasterId() {
		master_id = 0;
	}

private:
	std::string first_name;
	std::string last_name;
	uint32_t employee_id;
	
	static uint32_t master_id;
};

uint32_t Employee::master_id;

void dostuff() {

	glfwInit();
	GLFWwindow* window = glfwCreateWindow(640, 480, "Triangles", NULL, NULL);
	glfwMakeContextCurrent(window);
	gl3w
}

int main() {
	using namespace std;
	Employee::SetMasterId();

	Employee zak(string("Zak"), string("Kastl"));
	Employee lakin(string("Lakin"), string("Ragains"));

	vector<Employee> employeez;
	employeez.push_back(zak);
	employeez.push_back(lakin);

	auto wilsy = make_unique<Employee>(string("Wilsy"), string("Kastl"));
	auto nagaru = make_unique<Employee>(string("Kyosaki"), string("Nagaru"));

	vector<unique_ptr<Employee>> employees;
	employees.push_back(move(nagaru));
	employees.push_back(move(wilsy));

	dostuff();

	return 0;
}