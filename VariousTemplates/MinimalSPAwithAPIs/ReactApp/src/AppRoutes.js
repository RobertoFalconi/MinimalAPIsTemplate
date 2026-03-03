import TempisticheProcessi from "./view/DefinizioneTempistiche/tempisticaProcessi.jsx";
import DefinizioneTempistiche from "./view/DefinizioneTempistiche/DefinizioneTempistiche.jsx";
import Home from "./view/Home.jsx";
import ValiditàCampagna from "./view/DefinizioneTempistiche/validitaCampagna.jsx";
import MappaturaProcessi from "./view/MappaturaProcessi/mappaturaProcessi.jsx";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
  },
  {
    path: '/definizioneTempistiche',
    element: <DefinizioneTempistiche />,
  },
  {
    path: '/definizioneTempistiche/tempisticaProcessi',
    element: <TempisticheProcessi />,
  },
  {
    path: '/definizioneTempistiche/validitaCampagna',
    element: <ValiditàCampagna />,
  },
  {
    path: '/mappaturaProcessi',
    element: <MappaturaProcessi />,
  }
  
];

export default AppRoutes;
