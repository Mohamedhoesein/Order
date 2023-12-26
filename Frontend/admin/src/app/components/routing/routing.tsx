import { Routes, Route } from "react-router-dom";
import { MainContent } from "../../layout/content/main-content.tsx";
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
import { UnprotectedContent } from "../../layout/content/unprotected-content.tsx";
import { Verify } from "../../pages/verify/verify.tsx";

export const Routing = () => {
    return (
        <Routes>
            <Route path="login" element={<UnprotectedRoute><UnprotectedContent><LogIn/></UnprotectedContent></UnprotectedRoute>}/>
            <Route path="forgot-password" element={<UnprotectedRoute><UnprotectedContent><ForgotPassword/></UnprotectedContent></UnprotectedRoute>}/>
            <Route path="change-password/:id/:code" element={<UnprotectedContent><ChangedPassword/></UnprotectedContent>}/>
            <Route path="verify/:id/:code" element={<UnprotectedRoute><UnprotectedContent><Verify/></UnprotectedContent></UnprotectedRoute>}/>
            <Route path="not-found" element={<UnprotectedRoute><UnprotectedRoute><NotFound/></UnprotectedRoute></UnprotectedRoute>}/>
            <Route element={<ProtectedRoute><MainContent></MainContent></ProtectedRoute>}>
                <Route index element={<Index/>}/>
                <Route path="register" element={<Register/>}/>
                <Route path="logout" element={<Logout/>}/>
                <Route path="account" element={<UpdateAccount/>}/>
            </Route>
            <Route path="*" element={<NotFound/>}/>
        </Routes>
    );
};