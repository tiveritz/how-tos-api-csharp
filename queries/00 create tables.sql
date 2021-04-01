DROP DATABASE IF EXISTS HowTos;
CREATE DATABASE HowTos;
USE HowTos;


/* -- USER EDITABLE ---------- */
DROP TABLE IF EXISTS HowTos;
CREATE TABLE HowTos (
	id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(128),
    ts_create TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ts_update TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP 
);

DROP TABLE IF EXISTS HowTosUriIds;
CREATE TABLE HowTosUriIds (
	how_to_id INT,
    uri_id char(8),
    FOREIGN KEY (how_to_id) REFERENCES HowTos(id)
);

DROP TABLE IF EXISTS Steps;
CREATE TABLE Steps (
	id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(128),
    description VARCHAR(512),
    ts_create TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ts_update TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
    );
    
DROP TABLE IF EXISTS Sub;
CREATE TABLE Sub (
    step_id INT,
    explanation VARCHAR(2000),
    FOREIGN KEY (step_id) REFERENCES Steps(id)
);


/* -- MAPPING + ORDERING ---------- */
DROP TABLE IF EXISTS HowTosSteps;
CREATE TABLE HowTosSteps (
    how_to_id INT,
    step_id INT,
    pos INT,
    #PRIMARY KEY (how_to_id, pos),
    FOREIGN KEY (how_to_id) REFERENCES HowTos(id),
    FOREIGN KEY (step_id) REFERENCES Steps(id)
);

DROP TABLE IF EXISTS Super;
CREATE TABLE Super (
	super_id INT,
    step_id INT,
    pos INT,
    #PRIMARY KEY (super_id, pos),
    FOREIGN KEY (super_id) REFERENCES Steps(id),
    FOREIGN KEY (step_id) REFERENCES Steps(id)
);
