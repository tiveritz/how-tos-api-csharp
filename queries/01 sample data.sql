/* Create HowTos */
INSERT INTO HowTos (title) VALUES ('How to Run SQL Statements'); #1
INSERT INTO HowTos (title) VALUES ('How to Run Cloud Server'); #2
INSERT INTO HowTos (title) VALUES ('How to Go Nuts'); #3

/* Create HowTosUriIds */
INSERT INTO HowTosUriIds (how_to_id, uri_id) VALUES (1, "a9d8cd7a"); #1
INSERT INTO HowTosUriIds (how_to_id, uri_id) VALUES (2, "l9d86e5s"); #2
INSERT INTO HowTosUriIds (how_to_id, uri_id) VALUES (3, "pic9d876"); #3

/* Create Steps */
INSERT INTO Steps (title, description) VALUES ('1 SQL', 'Do Something'); #1
INSERT INTO Steps (title, description) VALUES ('2 SQL', 'Do Something'); #2
INSERT INTO Steps (title, description) VALUES ('2.1 SQL', 'Do Something'); #3
INSERT INTO Steps (title, description) VALUES ('2.2 SQL', 'Do Something'); #4
INSERT INTO Steps (title, description) VALUES ('2.2.1 SQL', 'Do Something'); #5
INSERT INTO Steps (title, description) VALUES ('2.2.2 SQL', 'Do Something'); #6
INSERT INTO Steps (title, description) VALUES ('3 SQL', 'Do Something'); #7
INSERT INTO Steps (title, description) VALUES ('4 SQL', 'Do Something'); #8

INSERT INTO Steps (title, description) VALUES ('1 How to Run Cloud Server', 'Do Something'); #9
INSERT INTO Steps (title, description) VALUES ('2 How to Run Cloud Server', 'Do Something'); #10
INSERT INTO Steps (title, description) VALUES ('3 How to Run Cloud Server', 'Do Something'); #11

INSERT INTO Steps (title, description) VALUES ('A wild step appears', 'Do Something'); #12

/* Create StepsIds */
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (1, "a93jdjc7"); #1
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (2, "d874djd9"); #2
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (3, "2ls98s7e"); #3
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (4, "djc847dj"); #4
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (5, "dj8d7f6e"); #5
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (6, "a00d9s8e"); #6
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (7, "yjd77s6e"); #7
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (8, "djf77r6e"); #8
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (9, "ka8d752g"); #9
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (10, "dj8s763k"); #10
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (11, "a09d74j6"); #11
INSERT INTO StepsUriIds (step_id, uri_id) VALUES (12, "5dsa6d5w"); #12

/* Assign Steps to HowTo */
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 1, 0);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 2, 1);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 7, 2);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 8, 3);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 9, 0);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 10, 1);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 11, 2);

/* Assign Steps to Super */
INSERT INTO Super (super_id, step_id, pos) VALUES (2, 3, 0);
INSERT INTO Super (super_id, step_id, pos) VALUES (2, 4, 1);
INSERT INTO Super (super_id, step_id, pos) VALUES (4, 5, 0);
INSERT INTO Super (super_id, step_id, pos) VALUES (4, 6, 1);

/* Assign Steps to StepsExplanations */
INSERT INTO StepsExplanations (step_id, explanation) VALUES (1, '1 SQL Explanation');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (7, '3 SQL Explanation');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (8, '4 SQL Explanation');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (9, '1 Cloud Server Explanation');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (10, '2 Cloud Server Explanation');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (11, '3 Cloud Server Explanation');

INSERT INTO StepsExplanations (step_id, explanation) VALUES (2, '2.1 Do Something');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (5, '2.2.1 Do Something');
INSERT INTO StepsExplanations (step_id, explanation) VALUES (6, '2.2.2 Do Something');