import { emailIconPath, nameIconPath, passwordIconPath, usernameIconPath } from "../../shared/components/auth/icons/icons";
import { SvgConfiguration } from "../../shared/models/configurations/UI/svg-configuration";

export const usernameIcon: SvgConfiguration = {
    path: usernameIconPath,
    viewBox: '0 0 35 35'
}

export const nameIcon: SvgConfiguration = {
    path: nameIconPath,
    viewBox: '0 0 35 33'
}

export const emailIcon: SvgConfiguration = {
  path: emailIconPath,
  viewBox: '0 0 35 27',
  marginTop: '1px'
};

export const passwordIcon: SvgConfiguration = {
  path: passwordIconPath,
  viewBox: '0 0 36 36'
}