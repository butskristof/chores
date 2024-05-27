import { useDark, useToggle } from '@vueuse/core';

export const useAppDarkMode = () => {
  const isDark = useDark({
    selector: 'html',
    attribute: 'color-scheme',
    valueDark: 'dark',
    valueLight: 'light',
  });
  const toggle = useToggle(isDark);

  return { isDark, toggle };
};
