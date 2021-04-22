#  ____________________________________
# |    _   _             _ _           |
# |   | |_(_)   _ __ _ _(_) |_ ____    |
# |   | __| |\ / /_ \ '_| | __|_  /    |
# |   | |_| | ' / __/ | | | |__/ /_    |
# |    \__|_|\_/\___|_| |_|\__/___/    |
# |____________________________________|
#
# 1. Database
# 2. User Editable
# 3. UriId Mapping
# 4. Order


# -----------------------------------------------------------------------------
#    1. Database
# -----------------------------------------------------------------------------
DROP DATABASE IF EXISTS HowTos;
CREATE DATABASE HowTos;
USE HowTos;


# -----------------------------------------------------------------------------
#    2. User Editable
# -----------------------------------------------------------------------------
DROP TABLE IF EXISTS HowTos;
CREATE TABLE HowTos (
	id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(128),
    ts_create TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ts_update TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP 
);

DROP TABLE IF EXISTS Steps;
CREATE TABLE Steps (
	id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(128),
    description VARCHAR(512),
    ts_create TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ts_update TIMESTAMP ON UPDATE CURRENT_TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
    );
    
DROP TABLE IF EXISTS StepsExplanations;
CREATE TABLE StepsExplanations (
    step_id INT,
    explanation VARCHAR(2000),
    CONSTRAINT FOREIGN KEY (step_id)
		REFERENCES Steps(id)
        ON DELETE CASCADE
);


# -----------------------------------------------------------------------------
#    3. UriId Mapping
# -----------------------------------------------------------------------------
DROP TABLE IF EXISTS HowTosUriIds;
CREATE TABLE HowTosUriIds (
	how_to_id INT,
    uri_id char(8) PRIMARY KEY,
    CONSTRAINT FOREIGN KEY (how_to_id)
		REFERENCES HowTos(id)
		ON DELETE CASCADE
);

DROP TABLE IF EXISTS StepsUriIds;
CREATE TABLE StepsUriIds (
	step_id INT,
    uri_id char(8) PRIMARY KEY,
    CONSTRAINT FOREIGN KEY (step_id)
		REFERENCES Steps(id)
        ON DELETE CASCADE  
);


# -----------------------------------------------------------------------------
#    4. Order
# -----------------------------------------------------------------------------
DROP TABLE IF EXISTS HowTosSteps;
CREATE TABLE HowTosSteps (
    how_to_id INT,
    step_id INT,
    pos INT,
    #PRIMARY KEY (how_to_id, pos),
    FOREIGN KEY (how_to_id)
		REFERENCES HowTos(id)
        ON DELETE CASCADE,
    CONSTRAINT FOREIGN KEY (step_id)
		REFERENCES Steps(id)
        ON DELETE CASCADE
);

DROP TABLE IF EXISTS Super;
CREATE TABLE Super (
	super_id INT,
    step_id INT,
    pos INT,
    #PRIMARY KEY (super_id, pos),
    CONSTRAINT FOREIGN KEY (super_id)
		REFERENCES Steps(id)
        ON DELETE CASCADE,
    FOREIGN KEY (step_id)
		REFERENCES Steps(id)
        ON DELETE CASCADE
);
