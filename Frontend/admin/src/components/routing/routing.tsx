import { Routes, Route } from "react-router-dom";
import { Content } from "../../layout/content/content";
import { Index } from "../../pages/index/index";
import { LogIn } from "../../pages/login/logIn";
import { Logout } from "../../pages/logout/logout";
import { Register } from "../../pages/register/register";
import { ForgotPassword } from "../../pages/forgot-password/forgot-password";
import { ChangedPassword } from "../../pages/changed-password/changed-password";
import { NotFound } from "../../pages/not-found/not-found";
import { UpdateAccount } from "../../pages/update-account/update-account";
import { ProtectedRoute } from "../protected-route/protected-route";
import { UnprotectedRoute } from "../unprotected-route/unprotected-route";

export const Routing = () => {
    return (
        <Routes>
            <Route path="login" element={<UnprotectedRoute><LogIn/></UnprotectedRoute>}/>
            <Route path="forgot-password" element={<UnprotectedRoute><ForgotPassword/></UnprotectedRoute>}/>
            <Route path="change-password/:id/:code" element={<ChangedPassword/>}/>
            <Route path="not-found" element={<UnprotectedRoute><NotFound/></UnprotectedRoute>}/>
            <Route element={<ProtectedRoute><Content></Content></ProtectedRoute>}>
                <Route index element={<Index/>}/>
                <Route path="register" element={<Register/>}/>
                <Route path="logout" element={<Logout/>}/>
                <Route path="account" element={<UpdateAccount/>}/>
            </Route>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
};