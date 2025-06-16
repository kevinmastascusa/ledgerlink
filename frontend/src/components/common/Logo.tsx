import React from 'react';
import { Box, SxProps, Theme } from '@mui/material';
import { useTheme } from '../../context/ThemeContext';

interface LogoProps {
  variant?: 'full' | 'icon';
  height?: number;
  sx?: SxProps<Theme>;
}

export const Logo: React.FC<LogoProps> = ({ variant = 'full', height = 40, sx }) => {
  const { darkMode } = useTheme();
  const color = darkMode ? '#90CAF9' : '#2196F3';

  const iconPath = (
    <path
      d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5"
      stroke={color}
      strokeWidth="2"
      fill="none"
    />
  );

  const textPath = (
    <path
      d="M24 8h-4v16h4V8zm-8 0H8v16h8V8zm-4 0H4v16h8V8z"
      fill={color}
    />
  );

  return (
    <Box
      sx={{
        display: 'inline-block',
        height,
        ...sx,
      }}
    >
      <svg
        width={variant === 'full' ? height * 3 : height}
        height={height}
        viewBox={variant === 'full' ? '0 0 96 32' : '0 0 32 32'}
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
      >
        {variant === 'full' ? (
          <>
            {iconPath}
            {textPath}
          </>
        ) : (
          iconPath
        )}
      </svg>
    </Box>
  );
}; 