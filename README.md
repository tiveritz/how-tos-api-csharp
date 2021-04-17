# How To's ![build-status](https://img.shields.io/docker/cloud/automated/tiveritz/how-tos-api) ![build-status](https://img.shields.io/docker/cloud/build/tiveritz/how-tos-api)
This Project consists of a collection of Web Applications that allow you to manage, edit and view How To's in the form of Step-By-Step procedures. An important key aspect is the modularity of the documentation. Reusable steps, pictures, explanations, links and so on are a core concept and the database is designed with that in mind.
[Swagger API documentation](https://api.tiveritz.at)

#### [Core API](https://github.com/tiveritz/how-tos-api)
The REST API that handles all database interactions on the documentation database.

#### [Administration](https://github.com/tiveritz/how-tos-administration)
A Website that allows users to manage the content. Consumes the Core API.

#### [Viewer](https://github.com/tiveritz/how-tos-viewer)
A Website that allows users to view the How To's. Consumes the Core API.

# Core API
* RESTful witch JSON payload
* HTTPS over SSL
* API versioning preparation
* Beautiful URLs
* GET ressources with required content
* GET pictures as links (ToDo: How to serve static files?)
* GET complete How To / Superstep tree with links to the steps (navigation in Viewer, Administration)
* GET should include links to previous and next step
* POST for content creation
* PATCH for content update
* DELETE with recycle bin (Restoring may be a bit tricky, because the content is very stricktly linked)
* Basic authentication (Over access token?)

| BASE                     | URL               | GET   | POST  | PUT   | PATCH | DELETE |
| ------------------------ | ----------------- | :---: | :---: | :---: | :---: | :----: |
| api.tiveritz.at/hwts/v1/ | statistics        |   ✓   |       |       |       |        |
| api.tiveritz.at/hwts/v1/ | howtos            |   ✓   |   ✓   |       |       |        |
| api.tiveritz.at/hwts/v1/ | howtos/{id}       |   ✓   |       |       |   ✓   |   ✓    |
| api.tiveritz.at/hwts/v1/ | steps             |   ✓   |   ✓   |       |       |        |
| api.tiveritz.at/hwts/v1/ | steps/{id}        |   ✓   |       |       |   ✓   |   ✓    |

#### statistics
GET statistical data about the available content<br/>

#### howtos
GET a list of all How To's<br/>
POST a new How To

#### howtos/{id}
GET a specific How To<br/>
PATCH (Update) information of a specific How To<br/>
DELETE a specific How To

#### steps
GET number of all available Steps with info if it is a Substep or Superstep<br/>
POST a new Step

#### steps/{id}
GET a specific step<br/>
PATCH (Update) information of a specific Step<br/>
DELETE a specific step

## Web Applications Diagram
![](./docs/server.png?raw=true "How To's server diagram")

## Continuous Integration Diagram
![](./docs/ci.png?raw=true "How To's CI diagram")

## UML
![](./docs/uml.png?raw=true "How To's UML")

## Database Model
![](./docs/db_model.png?raw=true "How To's Database Model")

## Documentation Core Features
Depending on the complexity and time of the project various features can be implemented. Sorted by priority descending.
* How To's include steps, sortable
* Steps used as Supersteps (wich linked Substeps) or Steps (with explanations), sortable
* Steps and Supersteps reusable (used by various Supersteps / How To's)
* How To's, Supersteps, Substeps have content (title, description, notes, to do's), depending on what makes sense
* Steps have Explanations
* API reference not by Database id but specific string or number (beautify URLs)
* Explanations have content (title, description, note, to do's)
* Explanations contain Explanation Modules
* Explanation Module Text
* Explanation Module Code
* Explanation Module Text contain links
* Explanation Module Text contain pictures (Depending on the Editor Framework -> research required
* Link can be external or internal
* Internal / External links can be managed
* Module Knowledge Base
* Module Knowledge Base contains Explanations
