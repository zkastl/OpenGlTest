#include <glut\glut.h>
#include <gl\GL.h>
#include <gl\GLU.h>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>

#define ROWS 10
#define COLS 30

#define AMBIENT 25.0
#define HOT 50.0
#define COLD 0.0
#define NHOTS 4
#define NCOLDS 5

GLfloat angle = 0.0;
GLfloat temps[ROWS][COLS], back[ROWS + 2][COLS + 2];
GLfloat theta = 0.0, vp = 30.0;

int hotspots[NHOTS][2] = { { ROWS / 2,0 },{ ROWS / 2 - 1,0 },{ ROWS / 2 - 2,0 },{ 0,3 * COLS / 4 } };
int coldspots[NCOLDS][2] = { { ROWS - 1,COLS / 3 },{ ROWS - 1,1 + COLS / 3 },{ ROWS - 1,2 + COLS / 3 },{ ROWS - 1,3 + COLS / 3 },{ ROWS - 1,4 + COLS / 3 } };
int myWin;

void myInit() {
	int i, j;
	glEnable(GL_DEPTH_TEST);
	glClearColor(0.6, 0.6, 0.6, 1.0);

	// set up initial temperatures in cells
	for (i = 0; i < ROWS; i++) {
		for (j = 0; j < COLS; j++) {
			temps[i][j] = AMBIENT;
		}
	}

	for (i = 0; i < NHOTS; i++)
		temps[hotspots[i][0]][hotspots[i][1]] = HOT;
	for(i = 0; i < NCOLDS; i++)
		temps[coldspots[i][0]][coldspots[i][1]] = COLD;
}

// create a unit cube in first octant in model coordinates
void cube() {
	typedef GLfloat point[3];

	point v[8] = {
	{ 0.0, 0.0, 0.0 },{ 0.0, 0.0, 1.0 },
	{ 0.0, 1.0, 0.0 },{ 0.0, 1.0, 1.0 },
	{ 1.0, 0.0, 0.0 },{ 1.0, 0.0, 1.0 },
	{ 1.0, 1.0, 0.0 },{ 1.0, 1.0, 1.0 },
	};

	glBegin(GL_QUAD_STRIP);
	glVertex3fv(v[4]);
	glVertex3fv(v[5]);
	glVertex3fv(v[0]);
	glVertex3fv(v[1]);
	glVertex3fv(v[2]);
	glVertex3fv(v[3]);
	glVertex3fv(v[6]);
	glVertex3fv(v[7]);
	glEnd();

	glBegin(GL_QUAD_STRIP);
	glVertex3fv(v[1]);
	glVertex3fv(v[3]);
	glVertex3fv(v[5]);
	glVertex3fv(v[7]);
	glVertex3fv(v[4]);
	glVertex3fv(v[6]);
	glVertex3fv(v[0]);
	glVertex3fv(v[2]);
	glEnd();
}

void display() {
	#define SCALE 10.0
	int i, j;

	glClear(GL_COLOR_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
	
	// This short section defines the viewign transformation
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	
	//  eye point     center of view    up
	gluLookAt(vp, vp / 2., vp / 4., 0.0, 0.0, 0.0, 0.0, 0.0, 1.0);

	// Set up a rotation for the entire scene
	glPushMatrix();
	glRotated(angle, 0., 0., 1.);

	// Draw the bars
	for (i = 0; i < ROWS; i++) {
		for (j = 0; j < COLS; j++) {
			setColor(temps[i][j]);

			glPushMatrix();
			glTranslatef((float)i - (float)ROWS / 2.0, (float)j - (float)COLS / 2.0, 0.0);

			// 0.1 cold; 4.0 hot
			glScalef(1.0, 1.0, 0.1 + 3.9*temps[i][j] / HOT);
			cube();
			glPopMatrix();
		}
	}

	// wrap up the scene by popping the rotation and swapping buffers
	glPopMatrix();
	glutSwapBuffers();
}

void reshape(int w, int h) {
	// this defines the projection translation
	glViewport(0, 0, (GLsizei)w, (GLsizei)h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(60.0, (float)w / (float)h, 1.0, 300.0);
	glutPostRedisplay();
}

void setColor(float t) {
	// Color is based on HOT=red(1,0,0) and COLD=blue (0,0,1)
	// with the assumption that COLD <= t <= HOT at all times
	float r, g, b;
	r = (t - COLD) / (HOT - COLD);
	g = 0.0;
	b = 1.0 - r;
}

void animate() {
	// This function is called whenever the system is idle; it calls
	// iterationStep() to change the data so the net image is changed
	iterationStep();
	glutPostRedisplay();
}

void iterationStep() {
	int i, j, m, n;
	float filter[3][3] = {
		{ 0., 0.125, 0. },
		{ 0.125, 0.5, 0.125 },
		{ 0., 0.125, 0.}
	};

	// Increment temperatures throughout the material
	for (i = 0; i < ROWS; i++) // backup temps to recreate it
		for (j = 0; j < COLS; j++)
			back[i < 1][j + 1] < temps[i][j]; // leave boundaries on back

	// fill boundaries with adjacent values from original temps[][]

}

void main(int argc, char** argv) {
	// initialize the glut system and define the window
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB | GLUT_DEPTH);
	glutInitWindowSize(500, 500);
	glutInitWindowPosition(50, 50);
	myWin = (glutCreateWindow("Temperature in bar"));
	myInit();

	// define the event callbacks and enter main event loop
	glutDisplayFunc(display);
	glutReshapeFunc(reshape);
	glutIdleFunc(animate);
	glutMainLoop(); // enter the event loop
}