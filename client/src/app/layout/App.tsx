import { Outlet } from "react-router-dom"
import LoadingIndicator from "./LoadingIndicator"

function App() {
  return (
    <>
    <LoadingIndicator variant="skeleton" /> 
     <h1 className ="font-bold bg-emerald-500 tracking-widest font-serif" >Your Live Mentor: FOCUS ON YOUNG GENERATION </h1>
     <Outlet />
    </>
  )
}

export default App
