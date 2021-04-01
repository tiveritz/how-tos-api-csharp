
/* Create HowTos */
INSERT INTO HowTos (title) VALUES ('How to Run SQL Statements'); #1
INSERT INTO HowTos (title) VALUES ('How to Drink Beer'); #2
INSERT INTO HowTos (title) VALUES ('How to Watch Rick and Morty'); #3



/* Create Steps */
INSERT INTO Steps (title, description) VALUES ('1 SQL', 'Do Something'); #1
INSERT INTO Steps (title, description) VALUES ('2 SQL', 'Do Something'); #2
INSERT INTO Steps (title, description) VALUES ('2.1 SQL', 'Do Something'); #3
INSERT INTO Steps (title, description) VALUES ('2.2 SQL', 'Do Something'); #4
INSERT INTO Steps (title, description) VALUES ('2.2.1 SQL', 'Do Something'); #5
INSERT INTO Steps (title, description) VALUES ('2.2.2 SQL', 'Do Something'); #6
INSERT INTO Steps (title, description) VALUES ('3 SQL', 'Do Something'); #7
INSERT INTO Steps (title, description) VALUES ('4 SQL', 'Do Something'); #8

INSERT INTO Steps (title, description) VALUES ('1 Drink Beer', 'Do Something'); #9
INSERT INTO Steps (title, description) VALUES ('2 Drink Beer', 'Do Something'); #10
INSERT INTO Steps (title, description) VALUES ('3 Drink Beer', 'Do Something'); #11



/* Assign Steps to HowTo */
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 1, 1);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 2, 2);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 7, 3);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (1, 8, 4);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 9, 1);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 10, 2);
INSERT INTO HowTosSteps (how_to_id, step_id, pos) VALUES (2, 11, 3);




/* Assign Steps to Super */
INSERT INTO Super (super_id, step_id, pos) VALUES (2, 3, 1);
INSERT INTO Super (super_id, step_id, pos) VALUES (2, 4, 2);
INSERT INTO Super (super_id, step_id, pos) VALUES (4, 5, 1);
INSERT INTO Super (super_id, step_id, pos) VALUES (4, 6, 2);



/* Assign Steps to Sub */
INSERT INTO Sub (step_id, explanation) VALUES (1, '1 SQL Explanation');
INSERT INTO Sub (step_id, explanation) VALUES (7, '3 SQL Explanation');
INSERT INTO Sub (step_id, explanation) VALUES (8, '4 SQL Explanation');
INSERT INTO Sub (step_id, explanation) VALUES (9, '1 Beer Explanation');
INSERT INTO Sub (step_id, explanation) VALUES (10, '2 Beer Explanation');
INSERT INTO Sub (step_id, explanation) VALUES (11, '3 Beer Explanation');