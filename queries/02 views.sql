#  ____________________________________
# |    _   _             _ _           |
# |   | |_(_)   _ __ _ _(_) |_ ____    |
# |   | __| |\ / /_ \ '_| | __|_  /    |
# |   | |_| | ' / __/ | | | |__/ /_    |
# |    \__|_|\_/\___|_| |_|\__/___/    |
# |____________________________________|
#
# 1. How To's Views
# 2. Steps Views


# -----------------------------------------------------------------------------
#    1. How To's Views
# -----------------------------------------------------------------------------
CREATE VIEW GetHowTos AS
	SELECT
		HowTosUriIds.uri_id,
        HowTos.title,
        HowTos.ts_create,
        HowTos.ts_update
	FROM HowTos
	JOIN HowTosUriIds ON HowTos.id=HowTosUriIds.how_to_id;

SELECT * FROM GetHowTos; #GetAllHowTosQuery
SELECT * FROM GetHowTos WHERE uri_id=@howToUriId; #GetHowToByIdQuery

# -----------------------------------------------------------------------------
#    2. Steps Views
# -----------------------------------------------------------------------------
CREATE VIEW GetSteps AS
	SELECT
		StepsUriIds.uri_id,
        Steps.title, Steps.ts_create,
        Steps.ts_update,
	CASE
		WHEN Steps.id IN (SELECT DISTINCT super_id FROM Super)
        THEN true
        ELSE false
	END AS is_super
	FROM Steps
	JOIN StepsUriIds ON Steps.id=StepsUriIds.step_id;

SELECT * FROM GetSteps ORDER BY ts_update DESC; #GetAllStepsQuery
SELECT * FROM GetSteps WHERE uri_id=@uriId; #GetStepByIdQuery
